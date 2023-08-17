using Nomnom.RaycastVisualization;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ダイブされていない & ダイブ中のプレイヤーの上にいるときのState
    /// </summary>
    public class TVNotDivedOnPlayerStayingState : IHamuState
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
        public TVNotDivedOnPlayerStayingState(ITransitionState<TVStateType> ts, TVObjectComponentController tvocc)
        {
            transitionState = ts;
            tvObjectComponent = tvocc;
        }
        
        public void Enter()
        {
            //物理演算で動かないようにする
            tvObjectComponent.Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            tvObjectComponent.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;

            //プレイヤーに追従させるため、自身をPlayerの子オブジェクトに設定する
            playerStateBehaviour = Object.FindObjectOfType<PlayerStateBehaviour>();
            tvObjectComponent.transform.parent = playerStateBehaviour.transform;

            playerPos = playerStateBehaviour.transform.position;
        }

        public void MyUpdate()
        {
            //下にプレイヤーがいるかチェックする
            //位置ずれしても大丈夫なように3本のRayで確認する
            var result = BoxCollider2DPositionCalculator.GetBoxCollider2DCenter(tvObjectComponent.BoxCollider2D);
            var centerHitPlayer = VisualPhysics2D.Raycast(result + new Vector2(0, -tvObjectComponent.transform.lossyScale.y / 2) + new Vector2(0, -0.01f),
                Vector2.down, 0.9f, tvObjectComponent.PlayerLayer);
            if (centerHitPlayer　&& playerStateBehaviour.PlayerStateMachine.IsDiving)
            {
                //ダイブ中のプレイヤーの上にいるなら座標を取得する
                playerPos = playerStateBehaviour.transform.position;
            }
            else
            {
                //下に地面やオハカがないかチェックする
                //こっちはisTouchingでOK！！
                var onGround = tvObjectComponent.Rigidbody2D.IsTouching(tvObjectComponent.GroundContactFilter2D);
                var onObject = tvObjectComponent.Rigidbody2D.IsTouching(tvObjectComponent.ObjectContactFilter2D);
                if(onGround || onObject) transitionState.TransitionState(TVStateType.NonDivedIdling);
                else transitionState.TransitionState(TVStateType.NonDivedFalling);
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
            tvObjectComponent.transform.parent = playerComponentController.DiveObjectTilemapTransform;
            tvObjectComponent.transform.position -= playerComponentController.transform.position - playerPos;
            playerStateBehaviour = null;
        }
    }
}
