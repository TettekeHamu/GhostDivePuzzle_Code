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
        /// オハカのコンポーネントをまとめたクラス
        /// </summary>
        private readonly TVObjectComponentController componentController;
        
        public TVPlayerDivingState(ITransitionState<TVStateType> ts, TVObjectComponentController tocc)
        {
            transitionState = ts;
            componentController = tocc;
        }
        
        public void Enter()
        {
            //動かす必要がない & 当たり判定をなくす
            componentController.Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            componentController.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            componentController.BoxCollider2D.enabled = false;
            //画像を非表示に
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
            componentController.SpriteRenderer.enabled = true;
        }
    }
}
