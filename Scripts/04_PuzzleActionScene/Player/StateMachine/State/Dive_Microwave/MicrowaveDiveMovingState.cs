using System;
using System.Threading;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// MicrowaveDiveMovingState
    /// </summary>
    public class MicrowaveDiveMovingState : IHamuState,ICollisionEnemy,IDisposable
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
        public MicrowaveDiveMovingState(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
            updateFuncManager = new PlayerDiveUpdateFuncManager(ts, pcc);
            exitFuncManager = new PlayerDiveExitFuncManager(ts, pcc);
            cancellationTokenSource = new CancellationTokenSource();
        }
        
        public void Enter()
        {
            elapsedTime = 0;
            cancellationTokenSource = new CancellationTokenSource();
            updateFuncManager.PlayMoveSe(cancellationTokenSource.Token).Forget();
        }

        public void MyUpdate()
        {
            //入力があったら電源を入れる
            if (PuzzleActionSceneInputController.Instance.ActionDiveAbility)
            {
                //まだ電気が通っていたら
                if (playerComponent.MicrowaveObject.ComponentController.IsRunningOutPower)
                {
                    var isRightOn = playerComponent.MicrowaveObject.ComponentController.ChangeRight();
                    playerComponent.ChangeCanDestroyEnemy(isRightOn);
                }
            }
            
            //入力方向に合わせてSpriteの向きを変更
            switch (PuzzleActionSceneInputController.Instance.MoveAxisKey.x)
            {
                case > 0:
                    playerComponent.AnimationManager.ChangeSpriteFlipX(false);
                    break;
                case < 0:
                    playerComponent.AnimationManager.ChangeSpriteFlipX(true);
                    break;
            }

            //下に地面がなければDivingFallStateに変更
            var onGround = updateFuncManager.CheckOnGround();
            if (!onGround)
            {
                transitionState.TransitionState(PlayerStateType.MicrowaveDivingFall);
                return;
            }

            //速度が一定以下ならDiveIdleStateに移行する
            if (PuzzleActionSceneInputController.Instance.MoveAxisKey.magnitude < 0.1f)
            {
                transitionState.TransitionState(PlayerStateType.MicrowaveDivingIdle);
                return;
            }
            
            //入力があればジャンプさせる
            if(PuzzleActionSceneInputController.Instance.JumpKey)
            {
                transitionState.TransitionState(PlayerStateType.MicrowaveDivingJumpUp);
                return;
            }

            //入力があればダイブを解除する
            if(PuzzleActionSceneInputController.Instance.StopDiveKey)
            {
                transitionState.TransitionState(PlayerStateType.Idle);
                exitFuncManager.StopDiving(
                    PlayerDiveType.DiveMicrowave,
                    playerComponent.MicrowaveObject.ComponentController.GroundLayer,
                    playerComponent.MicrowaveObject.ComponentController.ObjectLayer);
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
