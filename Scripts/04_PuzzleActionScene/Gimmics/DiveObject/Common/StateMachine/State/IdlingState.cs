using Nomnom.RaycastVisualization;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle.Refactoring
{
    /// <summary>
    /// 静止中のState
    /// </summary>
    public class IdlingState : IHamuState
    {
        /// <summary>
        /// Stateを変更する処理を持つインタフェース
        /// </summary>
        private readonly ITransitionState transitionState;
        /// <summary>
        /// コンポーネントをまとめたクラス
        /// </summary>
        private readonly DiveObjectComponentController componentController;
        /// <summary>
        /// 
        /// </summary>
        private IHamuState onPlayerState;
        /// <summary>
        /// 
        /// </summary>
        private IHamuState fallingState;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public IdlingState(ITransitionState ts, DiveObjectComponentController docc)
        {
            transitionState = ts;
            componentController = docc;
        }

        public void Initialize(IHamuState opState,IHamuState fState)
        {
            onPlayerState = opState;
            fallingState = fState;
        }
        
        public void Enter()
        {
            Debug.Log("静止！");
            //基本動かさないのでDynamic&FreezeAllにしておく
            //重力は効かせたいのでDynamicにしておく
            componentController.Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            componentController.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        public void MyUpdate()
        {
            //真下にプレイヤーがいるかチェック
            var result = BoxCollider2DPositionCalculator.GetBoxCollider2DCenter(componentController.BoxCollider2D);
            var length = -1 * componentController.transform.lossyScale.y / 2;
            var hitPlayer = VisualPhysics2D.Raycast
                    (result + new Vector2(0, length) + new Vector2(0, -0.01f),
                    Vector2.down, 0.5f, 
                    componentController.PlayerLayer);
            if (hitPlayer)
            {
                //ダイブしてたら追従させる
                var playerStateBehaviour = Object.FindObjectOfType<PlayerStateBehaviour>();
                if(!playerStateBehaviour.PlayerStateMachine.IsDiving) return;
                transitionState.TransitionState(onPlayerState);
                return;
            }
            
            //下にオブジェクトや地面がなければ落下させる
            var onGround = componentController.Rigidbody2D.IsTouching(componentController.GroundContactFilter2D);
            var onObject = componentController.Rigidbody2D.IsTouching(componentController.ObjectContactFilter2D);
            var onPlayer = componentController.Rigidbody2D.IsTouching(componentController.PlayerContactFilter2D);
            if(!onGround && !onObject && !onPlayer) transitionState.TransitionState(fallingState);
        }

        public void MyFixedUpdate()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}
