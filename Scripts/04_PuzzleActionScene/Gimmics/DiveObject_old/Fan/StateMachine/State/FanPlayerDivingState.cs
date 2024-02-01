using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// FanPlayerDivingState
    /// </summary>
    public class FanPlayerDivingState : IHamuState
    {
        /// <summary>
        /// Stateを変更する処理を持つインタフェース
        /// </summary>
        private readonly ITransitionState<FanStateType> transitionState;
        /// <summary>
        /// オハカのコンポーネントをまとめたクラス
        /// </summary>
        private readonly FanObjectComponentController componentController;
        
        public FanPlayerDivingState(ITransitionState<FanStateType> ts, FanObjectComponentController focc)
        {
            transitionState = ts;
            componentController = focc;
        }
        
        public void Enter()
        {
            //動かす必要がない & 当たり判定をなくす
            componentController.Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            componentController.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            componentController.BoxCollider2D.enabled = false;
            //Spriteを非表示に
            componentController.SpriteRenderer.enabled = false;
        }

        public void MyUpdate()
        {
            
        }

        public void MyFixedUpdate()
        {
            
        }

        public void Exit()
        {
            componentController.BoxCollider2D.enabled = true;
            //Spriteを表示させる
            componentController.SpriteRenderer.enabled = true;
        }
    }
}
