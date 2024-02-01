using Nomnom.RaycastVisualization;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// MicrowaveDivedIdlingState
    /// </summary>
    public class MicrowaveNotDivedIdlingState : IHamuState
    {
        /// <summary>
        /// Stateを変更する処理を持つインタフェース
        /// </summary>
        private readonly ITransitionState<MicrowaveStateType> transitionState;
        /// <summary>
        /// オハカのコンポーネントをまとめたクラス
        /// </summary>
        private readonly MicrowaveComponentController componentController;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public MicrowaveNotDivedIdlingState(ITransitionState<MicrowaveStateType> ts, MicrowaveComponentController mcc)
        {
            transitionState = ts;
            componentController = mcc;
        }
        
        public void Enter()
        {
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
            var hitPlayer = VisualPhysics2D.Raycast(result + new Vector2(0, length) + new Vector2(0, -0.01f), Vector2.down, 0.5f, componentController.PlayerLayer);
            if (hitPlayer)
            {
                //ダイブしてたら追従させる
                var playerStateBehaviour = Object.FindObjectOfType<PlayerStateBehaviour>();
                if(!playerStateBehaviour.PlayerStateMachine.IsDiving) return;
                transitionState.TransitionState(MicrowaveStateType.NonDivedOnPlayerStaying);
                return;
            }
            
            //下にオブジェクトや地面がなければ落下させる
            var onGround = componentController.Rigidbody2D.IsTouching(componentController.GroundContactFilter2D);
            var onObject = componentController.Rigidbody2D.IsTouching(componentController.ObjectContactFilter2D);
            var onPlayer = componentController.Rigidbody2D.IsTouching(componentController.PlayerContactFilter2D);
            if(!onGround && !onObject && !onPlayer) transitionState.TransitionState(MicrowaveStateType.NonDivedFalling);
        }

        public void MyFixedUpdate()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}
