using Nomnom.RaycastVisualization;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ダイブされていない & 地面の上にいるときのState
    /// </summary>
    public class TVNotDivedIdlingState : IHamuState
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
        public TVNotDivedIdlingState(ITransitionState<TVStateType> ts, TVObjectComponentController tvocc)
        {
            transitionState = ts;
            tvObjectComponent = tvocc;
        }
        
        public void Enter()
        {
            //動かす必要がないのでKinematic & 全て固定させる
            tvObjectComponent.Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            tvObjectComponent.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        public void MyUpdate()
        {
            var resultCenter = BoxCollider2DPositionCalculator.GetBoxCollider2DCenter(tvObjectComponent.BoxCollider2D);
            //下にプレイヤーがいないかチェックする
            var hitPlayer = VisualPhysics2D.Raycast(resultCenter + new Vector2(0, -tvObjectComponent.transform.lossyScale.y / 2) + new Vector2(0, -0.01f),
                Vector2.down, 0.9f, tvObjectComponent.PlayerLayer);
            if (hitPlayer)
            {
                var playerStateBehaviour = Object.FindObjectOfType<PlayerStateBehaviour>();
                //ダイブしていなかったらリターン
                if(!playerStateBehaviour.PlayerStateMachine.IsDiving) return;
                transitionState.TransitionState(TVStateType.NonDivedOnPlayerStaying);
                return;
            }

            //下に地面やオブジェクトがないかチェックする
            var resultVertices = BoxCollider2DPositionCalculator.GetBoxCollide2DVertices(tvObjectComponent.BoxCollider2D);
            var leftHitGround = VisualPhysics2D.Raycast(resultVertices[3] + new Vector2(0,-0.01f), 
                Vector2.down, 0.9f, tvObjectComponent.GroundLayer);
            var leftHitObject = VisualPhysics2D.Raycast(resultVertices[3] + new Vector2(0,-0.01f), 
                Vector2.down, 0.9f, tvObjectComponent.ObjectLayer);
            var leftHitPlayer = VisualPhysics2D.Raycast(resultVertices[3] + new Vector2(0,-0.01f), 
                Vector2.down, 0.9f, tvObjectComponent.PlayerLayer);
            var rightHitGround = VisualPhysics2D.Raycast(resultVertices[2] + new Vector2(0,-0.01f), 
                Vector2.down, 0.9f, tvObjectComponent.GroundLayer);
            var rightHitObject = VisualPhysics2D.Raycast(resultVertices[2] + new Vector2(0,-0.01f), 
                Vector2.down, 0.9f, tvObjectComponent.ObjectLayer);
            var rightHitPlayer = VisualPhysics2D.Raycast(resultVertices[3] + new Vector2(0,-0.01f), 
                Vector2.down, 0.9f, tvObjectComponent.PlayerLayer);
            var centerHitGround = VisualPhysics2D.Raycast(resultCenter + new Vector2(0, -tvObjectComponent.transform.lossyScale.y / 2) + new Vector2(0, -0.01f),
                Vector2.down, 0.9f, tvObjectComponent.GroundLayer);
            var centerHitObject = VisualPhysics2D.Raycast(resultCenter + new Vector2(0, -tvObjectComponent.transform.lossyScale.y / 2) + new Vector2(0, -0.01f),
                Vector2.down, 0.9f, tvObjectComponent.ObjectLayer);
            if (leftHitGround || leftHitObject ||  leftHitPlayer || rightHitGround || rightHitObject || rightHitPlayer ||centerHitGround || centerHitObject)
            {
                //設定中は何もしない
            }
            else
            {
                transitionState.TransitionState(TVStateType.NonDivedFalling);
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
