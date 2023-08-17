using Nomnom.RaycastVisualization;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 
    /// </summary>
    public class FanNotDivedOnPlayerStayingState : IHamuState
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
        public FanNotDivedOnPlayerStayingState(ITransitionState<FanStateType> ts, FanObjectComponentController focc)
        {
            transitionState = ts;
            fanObjectComponent = focc;
        }
        
        public void Enter()
        {
            //物理演算で動かないようにする
            fanObjectComponent.Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            fanObjectComponent.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;

            //プレイヤーに追従させるため、自身をPlayerの子オブジェクトに設定する
            playerStateBehaviour = Object.FindObjectOfType<PlayerStateBehaviour>();
            fanObjectComponent.transform.parent = playerStateBehaviour.transform;

            playerPos = playerStateBehaviour.transform.position;
        }

        public void MyUpdate()
        {
            //下にプレイヤーがいるかチェックする
            //位置ずれしても大丈夫なように3本のRayで確認する
            var result = BoxCollider2DPositionCalculator.GetBoxCollider2DCenter(fanObjectComponent.BoxCollider2D);
            var centerHitPlayer = VisualPhysics2D.Raycast(result+ new Vector2(0, -fanObjectComponent.transform.lossyScale.y / 2) + new Vector2(0, -0.01f), Vector2.down, 0.9f, fanObjectComponent.PlayerLayer);
            if (centerHitPlayer　&& playerStateBehaviour.PlayerStateMachine.IsDiving)
            {
                //ダイブ中のプレイヤーの上にいるなら座標を取得する
                playerPos = playerStateBehaviour.transform.position;
            }
            else
            {
                //下に地面やオハカがないかチェックする
                //こっちはisTouchingでOK！！
                var onGround = fanObjectComponent.Rigidbody2D.IsTouching(fanObjectComponent.GroundContactFilter2D);
                var onObject = fanObjectComponent.Rigidbody2D.IsTouching(fanObjectComponent.ObjectContactFilter2D);
                if(onGround || onObject) transitionState.TransitionState(FanStateType.NonDivedIdling);
                else transitionState.TransitionState(FanStateType.NonDivedFalling);
            }
        }

        public void MyFixedUpdate()
        {

        }

        public void Exit()
        {
            //親を元に戻してあげる
            //プレイヤーがダイブ終了した時点も子オブジェクトになっているため、ダイブ終了後もプレイヤーの頭上にオハカが乗ってしまうため位置を補正してあげる
            var playerComponentController = playerStateBehaviour.GetComponent<PlayerComponentController>();
            fanObjectComponent.transform.parent = playerComponentController.DiveObjectTilemapTransform;
            fanObjectComponent.transform.position -= playerComponentController.transform.position - playerPos;
            playerStateBehaviour = null;
        }
    }
}
