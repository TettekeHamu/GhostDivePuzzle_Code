using Nomnom.RaycastVisualization;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ダイブされていない & 静止しているときのState
    /// </summary>
    public class NotDivedIdlingState : IHamuState
    {
        /// <summary>
        /// Stateを変更する処理を持つインタフェース
        /// </summary>
        private readonly ITransitionState<TombStateType> transitionState;
        /// <summary>
        /// オハカのコンポーネントをまとめたクラス
        /// </summary>
        private readonly TombObjectComponentController tombObjectComponent;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public NotDivedIdlingState(ITransitionState<TombStateType> ts, TombObjectComponentController tocc)
        {
            transitionState = ts;
            tombObjectComponent = tocc;
        }
        
        public void Enter()
        {
            //基本動かさないのでDynamic&FreezeAllにしておく
            //重力は効かせたいのでDynamicにしておく
            tombObjectComponent.Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            tombObjectComponent.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        public void MyUpdate()
        {
            //真下にプレイヤーがいるかチェック
            var result = BoxCollider2DPositionCalculator.GetBoxCollider2DCenter(tombObjectComponent.BoxCollider2D);
            var length = -1 * tombObjectComponent.transform.lossyScale.y / 2;
            var hitPlayer = VisualPhysics2D.Raycast(result + new Vector2(0, length) + new Vector2(0, -0.01f), Vector2.down, 0.5f, tombObjectComponent.PlayerLayer);
            if (hitPlayer)
            {
                //ダイブしてたら追従させる
                var playerStateBehaviour = Object.FindObjectOfType<PlayerStateBehaviour>();
                if(!playerStateBehaviour.PlayerStateMachine.IsDiving) return;
                transitionState.TransitionState(TombStateType.NonDivedOnPlayerStaying);
                return;
            }
            
            //下にオブジェクトや地面がなければ落下させる
            var onGround = tombObjectComponent.Rigidbody2D.IsTouching(tombObjectComponent.GroundContactFilter2D);
            var onObject = tombObjectComponent.Rigidbody2D.IsTouching(tombObjectComponent.ObjectContactFilter2D);
            var onPlayer = tombObjectComponent.Rigidbody2D.IsTouching(tombObjectComponent.PlayerContactFilter2D);
            if(!onGround && !onObject && !onPlayer) transitionState.TransitionState(TombStateType.NonDivedFalling);

            /*
            var results = BoxCollider2DVerticesCalculator.GetBoxCollide2DVerticesAndCenter(tombObjectComponent.BoxCollider2D);
            //下にプレイヤーがいないかチェックする
            var hitPlayer = VisualPhysics2D.Raycast(results.Item2 + new Vector2(0, -tombObjectComponent.transform.lossyScale.y / 2) + new Vector2(0, -0.01f),
                Vector2.down, 0.9f, tombObjectComponent.PlayerLayer);
            if (hitPlayer)
            {
                var playerStateBehaviour = Object.FindObjectOfType<PlayerStateBehaviour>();
                //ダイブしていなかったらリターン
                if(!playerStateBehaviour.PlayerStateMachine.IsDiving) return;
                transitionState.TransitionState(TombStateType.NonDivedOnPlayerStaying);
                return;
            }

            //下に地面やオブジェクトがないかチェックする
            var leftHitGround = VisualPhysics2D.Raycast(results.Item1[3] + new Vector2(0,-0.01f), 
                Vector2.down, 0.9f, tombObjectComponent.GroundLayer);
            var leftHitObject = VisualPhysics2D.Raycast(results.Item1[3] + new Vector2(0,-0.01f), 
                Vector2.down, 0.9f, tombObjectComponent.ObjectLayer);
            var leftHitPlayer = VisualPhysics2D.Raycast(results.Item1[3] + new Vector2(0,-0.01f), 
                Vector2.down, 0.9f, tombObjectComponent.PlayerLayer);
            var rightHitGround = VisualPhysics2D.Raycast(results.Item1[2] + new Vector2(0,-0.01f), 
                Vector2.down, 0.9f, tombObjectComponent.GroundLayer);
            var rightHitObject = VisualPhysics2D.Raycast(results.Item1[2] + new Vector2(0,-0.01f), 
                Vector2.down, 0.9f, tombObjectComponent.ObjectLayer);
            var rightHitPlayer = VisualPhysics2D.Raycast(results.Item1[3] + new Vector2(0,-0.01f), 
                Vector2.down, 0.9f, tombObjectComponent.PlayerLayer);
            var centerHitGround = VisualPhysics2D.Raycast(results.Item2 + new Vector2(0, -tombObjectComponent.transform.lossyScale.y / 2) + new Vector2(0, -0.01f),
                Vector2.down, 0.9f, tombObjectComponent.GroundLayer);
            var centerHitObject = VisualPhysics2D.Raycast(results.Item2 + new Vector2(0, -tombObjectComponent.transform.lossyScale.y / 2) + new Vector2(0, -0.01f),
                Vector2.down, 0.9f, tombObjectComponent.ObjectLayer);
            if (leftHitGround || leftHitObject ||  leftHitPlayer || rightHitGround || rightHitObject || rightHitPlayer ||centerHitGround || centerHitObject)
            {
                //設定中は何もしない
            }
            else
            {
                transitionState.TransitionState(TombStateType.NonDivedFalling);
            }
            */
        }

        public void MyFixedUpdate()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}
