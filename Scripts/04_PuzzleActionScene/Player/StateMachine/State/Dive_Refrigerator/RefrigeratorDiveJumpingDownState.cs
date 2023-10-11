using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// RefrigeratorDiveJumpingDownState
    /// </summary>
    public class RefrigeratorDiveJumpingDownState : IHamuState,ICollisionEnemy
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
        public RefrigeratorDiveJumpingDownState(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
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
            //接地判定
            var onGround = updateFuncManager.CheckOnGround();
            if(onGround) transitionState.TransitionState(PlayerStateType.RefrigeratorDivingLand);
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
