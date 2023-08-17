using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ダイブ中に静止中のState
    /// </summary>
    public class DiveIdlingState : IHamuState,ICollisionEnemy
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
        public DiveIdlingState(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
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
            //近くにオソナエサキがあればDiveOfferingStateに変更
            var nearbyOfferingPlace = updateFuncManager.CheckNearOfferingPlace();
            if (nearbyOfferingPlace)
            {
                transitionState.TransitionState(PlayerStateType.DivingOffering);
                return;
            }

            //下に地面やオブジェクトがなければDivingFallStateに変更
            var onGround = updateFuncManager.CheckOnGround();
            if (!onGround)
            {
                transitionState.TransitionState(PlayerStateType.DivingFall);
                return;
            }

            //入力があればDiveMoveStateに移行
            if (PuzzleActionSceneInputController.Instance.MoveAxisKey.x is > 0.1f or < -0.1f)
            {
                transitionState.TransitionState(PlayerStateType.DivingMove);
                return;
            }
            
            //入力があればジャンプさせる
            if(PuzzleActionSceneInputController.Instance.JumpKey)
            {
                transitionState.TransitionState(PlayerStateType.DivingJumpUp);
                return;
            }
            
            //入力があればダイブを解除する
            if(PuzzleActionSceneInputController.Instance.StopDiveKey)
            {
                transitionState.TransitionState(PlayerStateType.Idle);
                exitFuncManager.StopDiving(
                    PlayerDiveType.DiveTomb,
                    playerComponent.TombObject.TombObjectComponent.GroundLayer,
                    playerComponent.TombObject.TombObjectComponent.ObjectLayer);
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
