using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 扇風機オブジェクトにダイブ中 & 待機中のState
    /// </summary>
    public class FanDiveIdlingState : IHamuState,ICollisionEnemy
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
        /// ダイブ開始時の機能をまとめたクラス
        /// </summary>
        private readonly PlayerDiveEnterFuncManager playerDiveEnterFuncManager;
        /// <summary>
        /// ダイブ終了時の機能をまとめたクラス
        /// </summary>
        private readonly PlayerDiveExitFuncManager playerDiveExitFuncManager;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public FanDiveIdlingState(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
            playerDiveEnterFuncManager = new PlayerDiveEnterFuncManager(ts, pcc);
            playerDiveExitFuncManager = new PlayerDiveExitFuncManager(ts, pcc);
        }
        
        public void Enter()
        {
            playerComponent.Rigidbody2D.velocity =Vector2.zero;
        }

        public void MyUpdate()
        {
            //下に地面がなければDivingFallStateに変更
            //var onGround = playerDiveEnterFuncManager.CheckOnGround();
            //if (!onGround)
            {
                transitionState.TransitionState(PlayerStateType.FanDivingJumpDown);
                return;
            }

            //入力があればDiveMoveStateに移行
            if (PuzzleActionSceneInputController.Instance.MoveAxisKey.x is > 0.1f or < -0.1f)
            {
                transitionState.TransitionState(PlayerStateType.FanDivingMove);
            }
            //入力があればダイブを解除する
            else if(PuzzleActionSceneInputController.Instance.StopDiveKey)
            {
                transitionState.TransitionState(PlayerStateType.Idle);
                playerDiveExitFuncManager.StopDiving(
                    PlayerDiveType.DiveFan,
                    playerComponent.FanObject.FanObjectComponentController.GroundLayer,
                    playerComponent.FanObject.FanObjectComponentController.ObjectLayer);
            }
            
            if (PuzzleActionSceneInputController.Instance.ActionDiveAbility)
            {
                //ジャンプさせる
                transitionState.TransitionState(PlayerStateType.FanDivingJumpUp);
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
