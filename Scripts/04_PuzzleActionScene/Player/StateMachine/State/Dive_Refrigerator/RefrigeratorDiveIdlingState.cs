using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 冷蔵庫にダイブ中 & 待機中のState
    /// </summary>
    public class RefrigeratorDiveIdlingState : IHamuState,ICollisionEnemy
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
        /// 
        /// </summary>
        private readonly PlayerDiveUpdateFuncManager updateFuncManager;
        /// <summary>
        /// ダイブ終了時の機能をまとめたクラス
        /// </summary>
        private readonly PlayerDiveExitFuncManager exitFuncManager;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public RefrigeratorDiveIdlingState(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
            updateFuncManager = new PlayerDiveUpdateFuncManager(ts, pcc);
            exitFuncManager = new PlayerDiveExitFuncManager(ts, pcc);
        }
        
        public void Enter()
        {
            //重力をなくす
            playerComponent.Rigidbody2D.velocity =Vector2.zero;
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
            
            //下に地面やオブジェクトがなければDivingFallStateに変更
            var onGround = updateFuncManager.CheckOnGround();
            if (!onGround)
            {
                transitionState.TransitionState(PlayerStateType.RefrigeratorDivingFall);
                return;
            }

            //入力があればDiveMoveStateに移行
            if (PuzzleActionSceneInputController.Instance.MoveAxisKey.x is > 0.1f or < -0.1f)
            {
                transitionState.TransitionState(PlayerStateType.RefrigeratorDivingMove);
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
        }

        public void MyFixedUpdate()
        {
            playerComponent.Rigidbody2D.velocity = Vector2.zero;
        }

        public void Exit()
        {
            
        }
        
        public void CollisionEnemy()
        {
            transitionState.TransitionState(PlayerStateType.Dead);
        }
    }
}
