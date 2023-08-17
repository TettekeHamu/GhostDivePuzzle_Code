using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// プレイヤーがダイブしているときのState
    /// </summary>
    public class TVPlayerDivingState : IHamuState
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
        public TVPlayerDivingState(ITransitionState<TVStateType> ts, TVObjectComponentController tvocc)
        {
            transitionState = ts;
            tvObjectComponent = tvocc;
        }
        
        public void Enter()
        {
            //動かす必要がない & 当たり判定をなくす
            tvObjectComponent.Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            tvObjectComponent.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            tvObjectComponent.BoxCollider2D.enabled = false;
        }

        public void MyUpdate()
        {
            
        }

        public void MyFixedUpdate()
        {
            
        }

        public void Exit()
        {
            tvObjectComponent.BoxCollider2D.enabled = true;
            tvObjectComponent.TurnOnTVLight(false);
        }
    }
}
