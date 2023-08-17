using Nomnom.RaycastVisualization;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ダイブされてない & プレイヤーの上にいるときのState（下のオブジェクトに追従させる）
    /// </summary>
    public class NotDivedOnPlayerStayingState : IHamuState
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
        public NotDivedOnPlayerStayingState(ITransitionState<TombStateType> ts, TombObjectComponentController tocc)
        {
            transitionState = ts;
            tombObjectComponent = tocc;
        }
        
        public void Enter()
        {
            //Dynamicだとうまく下にいるプレイヤーに追従しないため、Kinematicにする
            //プレイヤーに密接させたいため下方向に速度をかける
            tombObjectComponent.Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            tombObjectComponent.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;

            //プレイヤーに追従させるため、自身をPlayerの子オブジェクトに設定する
            playerStateBehaviour = Object.FindObjectOfType<PlayerStateBehaviour>();
            tombObjectComponent.transform.parent = playerStateBehaviour.transform;

            playerPos = playerStateBehaviour.transform.position;
        }

        public void MyUpdate()
        {
            
            //下にプレイヤーがいるかチェックする
            //位置ずれしても大丈夫なように3本のRayで確認する
            var result = BoxCollider2DPositionCalculator.GetBoxCollider2DCenter(tombObjectComponent.BoxCollider2D);
            var centerHitPlayer = VisualPhysics2D.Raycast(result + new Vector2(0, -tombObjectComponent.transform.lossyScale.y / 2) + new Vector2(0, -0.01f), Vector2.down, 0.9f, tombObjectComponent.PlayerLayer);
            if (centerHitPlayer　&& playerStateBehaviour.PlayerStateMachine.IsDiving)
            {
                //ダイブ中のプレイヤーの上にいるなら座標を取得する
                playerPos = playerStateBehaviour.transform.position;
            }
            else
            {
                //下に地面やオハカがないかチェックする
                var onGround = tombObjectComponent.Rigidbody2D.IsTouching(tombObjectComponent.GroundContactFilter2D);
                var onObject = tombObjectComponent.Rigidbody2D.IsTouching(tombObjectComponent.ObjectContactFilter2D);
                var onPlayer = tombObjectComponent.Rigidbody2D.IsTouching(tombObjectComponent.PlayerContactFilter2D); 
                if(onGround || onObject) transitionState.TransitionState(TombStateType.NonDivedIdling);
                else transitionState.TransitionState(TombStateType.NonDivedFalling);
            }
        }

        public void MyFixedUpdate()
        {
            //当たり判定を取るため、下方向に速度をかける
            //tombObjectComponent.Rigidbody2D.velocity = Vector2.down * 1;
        }

        public void Exit()
        {
            //親を元に戻してあげる
            //プレイヤーがダイブ終了した時点も子オブジェクトになっているため、ダイブ終了後もプレイヤーの頭上にオハカが乗ってしまうため位置を補正してあげる
            var playerComponentController = playerStateBehaviour.GetComponent<PlayerComponentController>();
            tombObjectComponent.transform.parent = playerComponentController.DiveObjectTilemapTransform;
            tombObjectComponent.transform.position -= playerComponentController.transform.position - playerPos;
            playerStateBehaviour = null;
        }
    }
}
