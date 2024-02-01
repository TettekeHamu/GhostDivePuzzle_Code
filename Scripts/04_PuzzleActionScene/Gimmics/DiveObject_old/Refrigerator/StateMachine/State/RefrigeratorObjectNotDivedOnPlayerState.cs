using Nomnom.RaycastVisualization;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ExampleObjectNotDivedOnPlayerState
    /// </summary>
    public class RefrigeratorObjectNotDivedOnPlayerState : IHamuState
    {
        /// <summary>
        /// Stateを変更する処理を持つインタフェース
        /// </summary>
        private readonly ITransitionState<RefrigeratorObjectStateType> transitionState;
        /// <summary>
        /// オハカのコンポーネントをまとめたクラス
        /// </summary>
        private readonly RefrigeratorObjectComponentController componentController;
        /// <summary>
        /// プレイヤー
        /// </summary>
        private PlayerStateBehaviour playerStateBehaviour;
        /// <summary>
        /// プレイヤーの座標
        /// </summary>
        private Vector3 playerPos;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public RefrigeratorObjectNotDivedOnPlayerState(ITransitionState<RefrigeratorObjectStateType> ts, RefrigeratorObjectComponentController rocc)
        {
            transitionState = ts;
            componentController = rocc;
        }
        
        public void Enter()
        {
            //componentController.Rigidbody2D.simulated = false;
            
            //Dynamicだとうまく下にいるプレイヤーに追従しないため、Kinematicにする
            //プレイヤーに密接させたいため下方向に速度をかける
            componentController.Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            componentController.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;

            //プレイヤーに追従させるため、自身をPlayerの子オブジェクトに設定する
            playerStateBehaviour = Object.FindObjectOfType<PlayerStateBehaviour>();
            componentController.transform.parent = playerStateBehaviour.transform;

            playerPos = playerStateBehaviour.transform.position;
        }

        public void MyUpdate()
        {
            
            //下にプレイヤーがいるかチェックする
            //位置ずれしても大丈夫なように3本のRayで確認する
            var result = BoxCollider2DPositionCalculator.GetBoxCollider2DCenter(componentController.BoxCollider2D);
            var centerHitPlayer = VisualPhysics2D.Raycast(result + new Vector2(0, -componentController.transform.lossyScale.y / 2) + new Vector2(0, -0.01f), Vector2.down, 0.9f, componentController.PlayerLayer);
            if (centerHitPlayer　&& playerStateBehaviour.PlayerStateMachine.IsDiving)
            {
                //ダイブ中のプレイヤーの上にいるなら座標を取得する
                playerPos = playerStateBehaviour.transform.position;
            }
            else
            {
                //下に地面やオハカがないかチェックする
                var onGround = componentController.Rigidbody2D.IsTouching(componentController.GroundContactFilter2D);
                var onObject = componentController.Rigidbody2D.IsTouching(componentController.ObjectContactFilter2D);
                var onPlayer = componentController.Rigidbody2D.IsTouching(componentController.PlayerContactFilter2D); 
                if(onGround || onObject) transitionState.TransitionState(RefrigeratorObjectStateType.NonDivedIdling);
                else transitionState.TransitionState(RefrigeratorObjectStateType.NonDivedFalling);
            }
        }

        public void MyFixedUpdate()
        {

        }

        public void Exit()
        {
            //componentController.Rigidbody2D.simulated = true;
            
            //親を元に戻してあげる
            //プレイヤーがダイブ終了した時点も子オブジェクトになっているため、ダイブ終了後もプレイヤーの頭上にオハカが乗ってしまうため位置を補正してあげる
            var playerComponentController = playerStateBehaviour.GetComponent<PlayerComponentController>();
            componentController.transform.parent = playerComponentController.DiveObjectTilemapTransform;
            componentController.transform.position -= playerComponentController.transform.position - playerPos;
            playerStateBehaviour = null;
        }
    }
}
