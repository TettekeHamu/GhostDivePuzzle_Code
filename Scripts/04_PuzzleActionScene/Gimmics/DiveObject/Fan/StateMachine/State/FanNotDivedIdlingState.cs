using Nomnom.RaycastVisualization;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 
    /// </summary>
    public class FanNotDivedIdlingState : IHamuState
    {
        /// <summary>
        /// Stateを変更する処理を持つインタフェース
        /// </summary>
        private readonly ITransitionState<FanStateType> transitionState;
        /// <summary>
        /// 扇風機のコンポーネントをまとめたクラス
        /// </summary>
        private readonly FanObjectComponentController fanObjectComponent;
        
        /// <summary>
        /// コンストラクター
        /// </summary>
        public FanNotDivedIdlingState(ITransitionState<FanStateType> ts, FanObjectComponentController focc)
        {
            transitionState = ts;
            fanObjectComponent = focc;
        }
        
        public void Enter()
        {
            //動かす必要がないのでKinematic & 全て固定させる
            fanObjectComponent.Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            fanObjectComponent.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        public void MyUpdate()
        {
            //下にプレイヤーがいないかチェックする
            var resultCenter = BoxCollider2DPositionCalculator.GetBoxCollider2DCenter(fanObjectComponent.BoxCollider2D);
            var hitPlayer = VisualPhysics2D.Raycast(resultCenter + new Vector2(0, -fanObjectComponent.transform.lossyScale.y / 2) + new Vector2(0, -0.01f),
                Vector2.down, 0.9f, fanObjectComponent.PlayerLayer);
            if (hitPlayer)
            {
                var playerStateBehaviour = Object.FindObjectOfType<PlayerStateBehaviour>();
                //ダイブしていなかったらリターン
                if(!playerStateBehaviour.PlayerStateMachine.IsDiving) return;
                transitionState.TransitionState(FanStateType.NonDivedOnPlayerStaying);
                return;
            }

            //下に地面やオブジェクトがないかチェックする
            var resultVertices = BoxCollider2DPositionCalculator.GetBoxCollide2DVertices(fanObjectComponent.BoxCollider2D);
            var leftHitGround = VisualPhysics2D.Raycast(resultVertices[3] + new Vector2(0,-0.01f), 
                Vector2.down, 0.9f, fanObjectComponent.GroundLayer);
            var leftHitObject = VisualPhysics2D.Raycast(resultVertices[3] + new Vector2(0,-0.01f), 
                Vector2.down, 0.9f, fanObjectComponent.ObjectLayer);
            var leftHitPlayer = VisualPhysics2D.Raycast(resultVertices[3] + new Vector2(0,-0.01f), 
                Vector2.down, 0.9f, fanObjectComponent.PlayerLayer);
            var rightHitGround = VisualPhysics2D.Raycast(resultVertices[2] + new Vector2(0,-0.01f), 
                Vector2.down, 0.9f, fanObjectComponent.GroundLayer);
            var rightHitObject = VisualPhysics2D.Raycast(resultVertices[2] + new Vector2(0,-0.01f), 
                Vector2.down, 0.9f, fanObjectComponent.ObjectLayer);
            var rightHitPlayer = VisualPhysics2D.Raycast(resultVertices[3] + new Vector2(0,-0.01f), 
                Vector2.down, 0.9f, fanObjectComponent.PlayerLayer);
            var centerHitGround = VisualPhysics2D.Raycast(resultCenter + new Vector2(0, -fanObjectComponent.transform.lossyScale.y / 2) + new Vector2(0, -0.01f),
                Vector2.down, 0.9f, fanObjectComponent.GroundLayer);
            var centerHitObject = VisualPhysics2D.Raycast(resultCenter + new Vector2(0, -fanObjectComponent.transform.lossyScale.y / 2) + new Vector2(0, -0.01f),
                Vector2.down, 0.9f, fanObjectComponent.ObjectLayer);
            if (leftHitGround || leftHitObject ||  leftHitPlayer || rightHitGround || rightHitObject || rightHitPlayer ||centerHitGround || centerHitObject)
            {
                //設定中は何もしない
            }
            else
            {
                transitionState.TransitionState(FanStateType.NonDivedFalling);
            }
        }

        public void MyFixedUpdate()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}
