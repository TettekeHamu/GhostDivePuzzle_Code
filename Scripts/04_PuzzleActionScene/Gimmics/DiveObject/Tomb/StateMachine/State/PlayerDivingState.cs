using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// プレイヤーにダイブされているときのState
    /// </summary>
    public class PlayerDivingState : IHamuState
    {
        /// <summary>
        /// Stateを変更する処理を持つインタフェース
        /// </summary>
        private readonly ITransitionState<TombStateType> transitionState;
        /// <summary>
        /// オハカのコンポーネントをまとめたクラス
        /// </summary>
        private readonly TombObjectComponentController tombObjectComponent;
        
        public PlayerDivingState(ITransitionState<TombStateType> ts, TombObjectComponentController tocc)
        {
            transitionState = ts;
            tombObjectComponent = tocc;
        }
        
        public void Enter()
        {
            //動かす必要がない & 当たり判定をなくす
            tombObjectComponent.Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            tombObjectComponent.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            tombObjectComponent.BoxCollider2D.enabled = false;
        }

        public void MyUpdate()
        {
            
        }

        public void MyFixedUpdate()
        {
            
        }

        public void Exit()
        {
            tombObjectComponent.BoxCollider2D.enabled = true;
        }
    }
}
