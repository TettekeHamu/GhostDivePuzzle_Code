using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 落下中のState
    /// </summary>
    public class RefrigeratorObjectNotDivedFallingState : IHamuState
    {
        /// <summary>
        /// Stateを変更する処理を持つインタフェース
        /// </summary>
        private readonly ITransitionState<RefrigeratorObjectStateType> transitionState;
        /// <summary>
        /// オハカのコンポーネントをまとめたクラス
        /// </summary>
        private readonly RefrigeratorObjectComponentController componentController;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public RefrigeratorObjectNotDivedFallingState(ITransitionState<RefrigeratorObjectStateType> ts, RefrigeratorObjectComponentController rocc)
        {
            transitionState = ts;
            componentController = rocc;
        }

        public void Enter()
        {
            //下方向のみ物理演算で動けるようにする
            componentController.Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            var constraints2D = componentController.Rigidbody2D.constraints;
            constraints2D |= RigidbodyConstraints2D.FreezePositionX;
            constraints2D |= RigidbodyConstraints2D.FreezeRotation;
            constraints2D &= ~RigidbodyConstraints2D.FreezePositionY;
            componentController.Rigidbody2D.constraints = constraints2D;
            //一旦重力をオフにする
            componentController.Rigidbody2D.gravityScale = 0;
        }

        public void MyUpdate()
        {
            //下にプレイヤーにいるかチェックする
            var onPlayer = componentController.Rigidbody2D.IsTouching(componentController.PlayerContactFilter2D);
            if (onPlayer)
            {
                var playerStateBehaviour = Object.FindObjectOfType<PlayerStateBehaviour>();
                //ダイブしていなかったらリターン
                if(!playerStateBehaviour.PlayerStateMachine.IsDiving) return;
                transitionState.TransitionState(RefrigeratorObjectStateType.NonDivedOnPlayerStaying);
                return;
            }

            //下にオブジェクトや地面がないかチェックする
            var onGround = componentController.Rigidbody2D.IsTouching(componentController.GroundContactFilter2D);
            var onObject = componentController.Rigidbody2D.IsTouching(componentController.ObjectContactFilter2D);
            if(onGround || onObject) transitionState.TransitionState(RefrigeratorObjectStateType.NonDivedIdling);
        }

        public void MyFixedUpdate()
        {
            componentController.Rigidbody2D.velocity = Vector2.down * 10f;
        }

        public void Exit()
        {
            componentController.Rigidbody2D.velocity = Vector2.zero;
            //重力を元に戻す
            componentController.Rigidbody2D.gravityScale = 1;
        }
    }
}
