using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 
    /// </summary>
    public class MicrowavePlayerDivingState : IHamuState
    {
        /// <summary>
        /// Stateを変更する処理を持つインタフェース
        /// </summary>
        private readonly ITransitionState<MicrowaveStateType> transitionState;
        /// <summary>
        /// オハカのコンポーネントをまとめたクラス
        /// </summary>
        private readonly MicrowaveComponentController componentController;
        
        public MicrowavePlayerDivingState(ITransitionState<MicrowaveStateType> ts, MicrowaveComponentController mcc)
        {
            transitionState = ts;
            componentController = mcc;
        }
        
        public void Enter()
        {
            //動かす必要がない & 当たり判定をなくす
            componentController.Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            componentController.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            componentController.BoxCollider2D.enabled = false;
            componentController.CircleCollider2D.enabled = false;
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
            componentController.CircleCollider2D.enabled = true;
            componentController.BoxCollider2D.enabled = true;
            //Spriteを表示させる
            componentController.SpriteRenderer.enabled = true;
            //電源を切る
            componentController.ChangeRight(false);
        }
    }
}
