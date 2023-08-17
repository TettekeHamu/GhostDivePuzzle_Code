using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 扇風機オブジェクトにダイブ中 & 下に落下中のState
    /// </summary>
    public class FanDiveJumpingDownState : IHamuState,ICollisionEnemy
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
        private readonly PlayerDiveEnterFuncManager playerDiveEnterFuncManager;
        /// <summary>
        /// 経過時間
        /// </summary>
        private float elapsedTime;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public FanDiveJumpingDownState(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
            playerDiveEnterFuncManager = new PlayerDiveEnterFuncManager(ts, pcc);
        }
        
        public void Enter()
        {
            elapsedTime = 0;
        }

        public void MyUpdate()
        {
            //移動させる、Rigidbodyで移動させると隙間が通れなくなるので注意
            playerComponent.transform.position +=
                new Vector3(PuzzleActionSceneInputController.Instance.MoveAxisKey.x, 0, 0).normalized * (playerComponent.MoveSpeed / 2 * Time.deltaTime);
            
            //var onGround = playerDiveEnterFuncManager.CheckOnGround();
            //if (onGround) transitionState.TransitionState(PlayerStateType.FanDivingLand);
        }

        public void MyFixedUpdate()
        {
            playerComponent.Rigidbody2D.velocity = Vector2.down * (Mathf.Pow(1.2f, elapsedTime * 15) + 1);
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
