using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 扇風機オブジェクトにダイブ中 & 上に上昇中のState
    /// </summary>
    public class FanDiveJumpingUpState : IHamuState,ICollisionEnemy
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
        public FanDiveJumpingUpState(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
            playerDiveEnterFuncManager = new PlayerDiveEnterFuncManager(ts, pcc);
        }
        
        public void Enter()
        {
            //Debug.Log("ジャンプ開始");
            elapsedTime = 0;
        }

        public void MyUpdate()
        {
            //横方向に移動させる、Rigidbodyで移動させると隙間が通れなくなるので注意
            playerComponent.transform.position += new Vector3(PuzzleActionSceneInputController.Instance.MoveAxisKey.x, 0, 0).normalized * (playerComponent.MoveSpeed / 2 * Time.deltaTime);
           
            //縦方向に上昇させる、velocityだと上にあるオブジェクトのbodyTypeを変更する必要があるためtransform.positionで実装
            var upSpeed = (8 - Mathf.Pow(1.2f, elapsedTime * 10) + 1);
            playerComponent.transform.position += Vector3.up * upSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            if (upSpeed <= 0) transitionState.TransitionState(PlayerStateType.FanDivingJumpIdle);
        }

        public void MyFixedUpdate()
        {
            /*
            playerComponent.Rigidbody2D.velocity = Vector2.up * (8 - Mathf.Pow(1.2f, elapsedTime * 10) + 1);
            elapsedTime += Time.fixedDeltaTime;
            if(playerComponent.Rigidbody2D.velocity.y <= 0) transitionState.TransitionState(PlayerStateType.FanDivingJumpIdle);
            */
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
