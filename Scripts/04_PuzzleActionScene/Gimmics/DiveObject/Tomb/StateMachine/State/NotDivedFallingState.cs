using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ダイブされていない & 落ちているときのState
    /// </summary>
    public class NotDivedFallingState : IHamuState
    {
        /// <summary>
        /// Stateを変更する処理を持つインタフェース
        /// </summary>
        private readonly ITransitionState<TombStateType> transitionState;
        /// <summary>
        /// オハカのコンポーネントをまとめたクラス
        /// </summary>
        private readonly TombObjectComponentController tombObjectComponent;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public NotDivedFallingState(ITransitionState<TombStateType> ts, TombObjectComponentController tocc)
        {
            transitionState = ts;
            tombObjectComponent = tocc;
        }

        public void Enter()
        {
            //下方向のみ物理演算で動けるようにする
            tombObjectComponent.Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            var constraints2D = tombObjectComponent.Rigidbody2D.constraints;
            constraints2D |= RigidbodyConstraints2D.FreezePositionX;
            constraints2D |= RigidbodyConstraints2D.FreezeRotation;
            constraints2D &= ~RigidbodyConstraints2D.FreezePositionY;
            tombObjectComponent.Rigidbody2D.constraints = constraints2D;
            //一旦重力をオフにする
            tombObjectComponent.Rigidbody2D.gravityScale = 0;
        }

        public void MyUpdate()
        {
            //下にプレイヤーにいるかチェックする
            var onPlayer = tombObjectComponent.Rigidbody2D.IsTouching(tombObjectComponent.PlayerContactFilter2D);
            if (onPlayer)
            {
                var playerStateBehaviour = Object.FindObjectOfType<PlayerStateBehaviour>();
                //ダイブしていなかったらリターン
                if(!playerStateBehaviour.PlayerStateMachine.IsDiving) return;
                transitionState.TransitionState(TombStateType.NonDivedOnPlayerStaying);
                return;
            }

            //下にオブジェクトや地面がないかチェックする
            var onGround = tombObjectComponent.Rigidbody2D.IsTouching(tombObjectComponent.GroundContactFilter2D);
            var onObject = tombObjectComponent.Rigidbody2D.IsTouching(tombObjectComponent.ObjectContactFilter2D);
            if(onGround || onObject) transitionState.TransitionState(TombStateType.NonDivedIdling);
        }

        public void MyFixedUpdate()
        {
            tombObjectComponent.Rigidbody2D.velocity = Vector2.down * 10f;
        }

        public void Exit()
        {
            tombObjectComponent.Rigidbody2D.velocity = Vector2.zero;
            //重力を元に戻す
            tombObjectComponent.Rigidbody2D.gravityScale = 1;
        }
    }
}
