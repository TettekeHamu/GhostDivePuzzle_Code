using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    public class TVDiveJumpingDownState : IHamuState,ICollisionEnemy
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
        /// 経過時間
        /// </summary>
        private float elapsedTime;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public TVDiveJumpingDownState(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
            updateFuncManager = new PlayerDiveUpdateFuncManager(ts, pcc);
        }
        
        public void Enter()
        {
            elapsedTime = 0;
        }

        public void MyUpdate()
        {
            //接地判定
            var onGround = updateFuncManager.CheckOnGround();
            if(onGround) transitionState.TransitionState(PlayerStateType.TVDivingLand);
        }

        public void MyFixedUpdate()
        {
            //左右に移動させるかつ下方向に落下させる
            var horizontalVec = PuzzleActionSceneInputController.Instance.MoveAxisKey.x * playerComponent.MoveSpeed / 2f;
            var fallSpeed = -10 * elapsedTime - 5;
            if (fallSpeed <= -10) fallSpeed = -10; 
            //Debug.Log(fallSpeed);
            playerComponent.Rigidbody2D.velocity = new Vector2(horizontalVec, fallSpeed);
            elapsedTime += Time.fixedDeltaTime;
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
