using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle.Refactoring
{
    /// <summary>
    /// プレイヤーにダイブされているときのState
    /// </summary>
    public class PlayerDivingState : IHamuState
    {
        /// <summary>
        /// オハカのコンポーネントをまとめたクラス
        /// </summary>
        private readonly DiveObjectComponentController componentController;
       
        /// <summary>
        /// コンストラクター
        /// </summary>
        public PlayerDivingState(DiveObjectComponentController docc)
        {
            componentController = docc;
        }

        public void Enter()
        {
            Debug.Log("プレイヤーにダイブされました");
            //物理演算で動かす必要がない & 当たり判定をなくす
            componentController.Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            componentController.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            componentController.BoxCollider2D.enabled = false;
            componentController.CircleCollider2D.enabled = false;
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
            OnExit();
            componentController.BoxCollider2D.enabled = true;
            componentController.CircleCollider2D.enabled = true;
            componentController.SpriteRenderer.enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnEnter()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnExit()
        {
            
        }
    }
}
