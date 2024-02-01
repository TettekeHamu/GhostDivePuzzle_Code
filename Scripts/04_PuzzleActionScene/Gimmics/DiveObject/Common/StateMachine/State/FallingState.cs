using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle.Refactoring
{
    /// <summary>
    /// 落下中のState
    /// </summary>
    public class FallingState : IHamuState
    {
        /// <summary>
        /// Stateを変更する処理を持つインタフェース
        /// </summary>
        private readonly ITransitionState transitionState;
        /// <summary>
        /// ダイブオブジェクトのコンポーネントをまとめたクラス
        /// </summary>
        private readonly DiveObjectComponentController componentController;
        /// <summary>
        /// 遷移先のState
        /// </summary>
        private IHamuState onPlayerState;
        /// <summary>
        /// 
        /// </summary>
        private IHamuState idlingState;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public FallingState(ITransitionState ts, DiveObjectComponentController docc)
        {
            transitionState = ts;
            componentController = docc;
        }

        
        public void Initialize(IHamuState opState, IHamuState iState)
        {
            onPlayerState = opState;
            idlingState = iState;
        }

        public void Enter()
        {
            Debug.Log("落下！");
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
                if (!playerStateBehaviour.PlayerStateMachine.IsDiving) return;
                transitionState.TransitionState(onPlayerState);
                return;
            }

            //下にオブジェクトや地面がないかチェックする
            var onGround = componentController.Rigidbody2D.IsTouching(componentController.GroundContactFilter2D);
            var onObject = componentController.Rigidbody2D.IsTouching(componentController.ObjectContactFilter2D);
            if (onGround || onObject) transitionState.TransitionState(idlingState);
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
