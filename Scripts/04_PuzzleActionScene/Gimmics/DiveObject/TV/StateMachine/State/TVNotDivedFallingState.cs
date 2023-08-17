using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ダイブされていない & 落下中のState
    /// </summary>
    public class TVNotDivedFallingState : IHamuState
    {
        /// <summary>
        /// Stateを変更する処理を持つインタフェース
        /// </summary>
        private readonly ITransitionState<TVStateType> transitionState;
        /// <summary>
        /// TVオブジェクトのコンポーネントをまとめたクラス
        /// </summary>
        private readonly TVObjectComponentController tvObjectComponent;
        
        /// <summary>
        /// コンストラクター
        /// </summary>
        public TVNotDivedFallingState(ITransitionState<TVStateType> ts, TVObjectComponentController tvocc)
        {
            transitionState = ts;
            tvObjectComponent = tvocc;
        }
        
        public void Enter()
        {
            //下方向のみ物理演算で動けるようにする
            tvObjectComponent.Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            var constraints2D = tvObjectComponent.Rigidbody2D.constraints;
            constraints2D |= RigidbodyConstraints2D.FreezePositionX;
            constraints2D |= RigidbodyConstraints2D.FreezeRotation;
            constraints2D &= ~RigidbodyConstraints2D.FreezePositionY;
            tvObjectComponent.Rigidbody2D.constraints = constraints2D;
        }

        public void MyUpdate()
        {
            //下にプレイヤーにいるかチェックする
            var onPlayer = tvObjectComponent.Rigidbody2D.IsTouching(tvObjectComponent.PlayerContactFilter2D);
            if (onPlayer)
            {
                var playerStateBehaviour = Object.FindObjectOfType<PlayerStateBehaviour>();
                //ダイブしていなかったらリターン
                if(!playerStateBehaviour.PlayerStateMachine.IsDiving) return;
                transitionState.TransitionState(TVStateType.NonDivedOnPlayerStaying);
                return;
            }

            //下にオブジェクトや地面がないかチェックする
            var onGround = tvObjectComponent.Rigidbody2D.IsTouching(tvObjectComponent.GroundContactFilter2D);
            var onObject = tvObjectComponent.Rigidbody2D.IsTouching(tvObjectComponent.ObjectContactFilter2D);
            if(onGround || onObject) transitionState.TransitionState(TVStateType.NonDivedIdling);
        }

        public void MyFixedUpdate()
        {
            //下に落下させる
            tvObjectComponent.Rigidbody2D.velocity = Vector2.down * 5f;
        }

        public void Exit()
        {
            tvObjectComponent.Rigidbody2D.velocity = Vector2.zero;
        }
    }
}
