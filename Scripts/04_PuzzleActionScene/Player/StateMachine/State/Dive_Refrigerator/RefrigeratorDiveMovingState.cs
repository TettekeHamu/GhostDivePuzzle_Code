using System;
using System.Threading;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 冷蔵庫にダイブ中 & 移動中のState
    /// </summary>
    public class RefrigeratorDiveMovingState : IHamuState,ICollisionEnemy,IDisposable
    {
        /// <summary>
        /// Stateを変更させる処理を持つインタフェース
        /// </summary>
        private readonly ITransitionState<PlayerStateType> transitionState;
        /// <summary>
        /// プレイヤーのコンポーネントをまとめたクラス
        /// </summary>
        private readonly PlayerComponentController playerComponent;
        /// <summary>
        /// ダイブ時の機能をまとめたクラス
        /// </summary>
        private readonly PlayerDiveUpdateFuncManager updateFuncManager;
        /// <summary>
        /// ダイブ終了時の機能をまとめたクラス
        /// </summary>
        private readonly PlayerDiveExitFuncManager exitFuncManager;
        /// <summary>
        /// 
        /// </summary>
        private CancellationTokenSource cancellationTokenSource;
        /// <summary>
        /// 経過時間
        /// </summary>
        private float elapsedTime;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public RefrigeratorDiveMovingState(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
            updateFuncManager = new PlayerDiveUpdateFuncManager(ts, pcc);
            exitFuncManager = new PlayerDiveExitFuncManager(ts, pcc);
            cancellationTokenSource = new CancellationTokenSource();
        }
        
        public void Enter()
        {
            cancellationTokenSource = new CancellationTokenSource();
            updateFuncManager.PlayMoveSe(cancellationTokenSource.Token).Forget();
            elapsedTime = 0;
        }

        public void MyUpdate()
        {
            //入力があったら電源を入れる
            if (PuzzleActionSceneInputController.Instance.ActionDiveAbility)
            {
                //まだ電気が通っていたら
                if (playerComponent.RefrigeratorObject.ComponentController.IsRunningOutPower)
                {
                    var isRightOn = playerComponent.RefrigeratorObject.ComponentController.ChangeRight();
                    playerComponent.ChangeCanDestroyEnemy(isRightOn);
                }
            }
            
            //入力方向に合わせてSpriteの向きを変更
            switch (PuzzleActionSceneInputController.Instance.MoveAxisKey.x)
            {
                case > 0:
                    playerComponent.AnimationManager.ChangeSpriteFlipX(true);
                    break;
                case < 0:
                    playerComponent.AnimationManager.ChangeSpriteFlipX(false);
                    break;
            }

            //下に地面がなければDivingFallStateに変更
            var onGround = updateFuncManager.CheckOnGround();
            if (!onGround)
            {
                transitionState.TransitionState(PlayerStateType.RefrigeratorDivingFall);
                return;
            }

            //速度が一定以下ならDiveIdleStateに移行する
            if (PuzzleActionSceneInputController.Instance.MoveAxisKey.magnitude < 0.1f)
            {
                transitionState.TransitionState(PlayerStateType.RefrigeratorDivingIdle);
                return;
            }
            
            //入力があればジャンプさせる
            if(PuzzleActionSceneInputController.Instance.JumpKey)
            {
                transitionState.TransitionState(PlayerStateType.RefrigeratorDivingJumpUp);
                return;
            }

            //入力があればダイブを解除する
            if(PuzzleActionSceneInputController.Instance.StopDiveKey)
            {
                transitionState.TransitionState(PlayerStateType.Idle);
                exitFuncManager.StopDiving(
                    PlayerDiveType.DiveRefrigerator,
                    playerComponent.RefrigeratorObject.ComponentController.GroundLayer,
                    playerComponent.RefrigeratorObject.ComponentController.ObjectLayer);
            }
            
            //加速に使う経過時間を加算
            if (elapsedTime <= 1) elapsedTime += Time.deltaTime * 1.5f;
            else elapsedTime = 1;
        }

        public void MyFixedUpdate()
        {
            //移動させる
            updateFuncManager.MovePlayer(playerComponent.MoveSpeed * elapsedTime,4.0f);
        }

        public void Exit()
        {
            cancellationTokenSource.Cancel();
        }
        
        public void CollisionEnemy()
        {
            transitionState.TransitionState(PlayerStateType.Dead);
        }

        public void Dispose()
        {
            cancellationTokenSource?.Dispose();
        }
    }
}
