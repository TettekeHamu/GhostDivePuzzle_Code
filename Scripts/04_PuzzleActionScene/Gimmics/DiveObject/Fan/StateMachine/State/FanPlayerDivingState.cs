using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 
    /// </summary>
    public class FanPlayerDivingState : IHamuState
    {
        /// <summary>
        /// Stateを変更する処理を持つインタフェース
        /// </summary>
        private readonly ITransitionState<FanStateType> transitionState;
        /// <summary>
        /// TVオブジェクトのコンポーネントをまとめたクラス
        /// </summary>
        private readonly FanObjectComponentController fanObjectComponent;
        
        /// <summary>
        /// コンストラクター
        /// </summary>
        public FanPlayerDivingState(ITransitionState<FanStateType> ts, FanObjectComponentController focc)
        {
            transitionState = ts;
            fanObjectComponent = focc;
        }
        
        public void Enter()
        {
            //動かす必要がない & 当たり判定をなくす
            fanObjectComponent.Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            fanObjectComponent.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            fanObjectComponent.BoxCollider2D.enabled = false;
        }

        public void MyUpdate()
        {
            
        }

        public void MyFixedUpdate()
        {
            
        }

        public void Exit()
        {
            fanObjectComponent.BoxCollider2D.enabled = true;
        }
    }
}
