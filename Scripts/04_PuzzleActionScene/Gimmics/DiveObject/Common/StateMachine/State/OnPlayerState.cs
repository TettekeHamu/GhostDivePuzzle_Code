using Nomnom.RaycastVisualization;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle.Refactoring
{
    /// <summary>
    /// プレイヤーの上にいるときのState
    /// </summary>
    public class OnPlayerState : IHamuState
    {
        /// <summary>
        /// Stateを変更する処理を持つインタフェース
        /// </summary>
        private readonly ITransitionState transitionState;
        /// <summary>
        /// オハカのコンポーネントをまとめたクラス
        /// </summary>
        private readonly DiveObjectComponentController componentController;
        /// <summary>
        /// 
        /// </summary>
        private IHamuState idlingState;
        /// <summary>
        /// 
        /// </summary>
        private IHamuState fallingState;
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
        public OnPlayerState(ITransitionState ts, DiveObjectComponentController docc)
        {
            transitionState = ts;
            componentController = docc;
        }

        public void Initialize(IHamuState iState, IHamuState fState)
        {
            idlingState = iState;
            fallingState = fState;
        }
        
        public void Enter()
        {
            Debug.Log("プレイヤーの上に静止！");
            componentController.Rigidbody2D.simulated = false;
            
            //Dynamicだとうまく下にいるプレイヤーに追従しないため、Kinematicにする
            //プレイヤーに密接させたいため下方向に速度をかける
            componentController.Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
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
            var centerHitPlayer =
                VisualPhysics2D.Raycast
                    (result + new Vector2(0, -componentController.transform.lossyScale.y / 2) + new Vector2(0, -0.01f),
                    Vector2.down, 
                    0.9f, 
                    componentController.PlayerLayer);
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
                if(onGround || onObject) transitionState.TransitionState(idlingState);
                else transitionState.TransitionState(fallingState);
            }
        }

        public void MyFixedUpdate()
        {
            //当たり判定を取るため、下方向に速度をかける
            //tombObjectComponent.Rigidbody2D.velocity = Vector2.down * 1;
        }

        public void Exit()
        {
            componentController.Rigidbody2D.simulated = true;
            //親を元に戻してあげる
            //プレイヤーがダイブ終了した時点も子オブジェクトになっているため、ダイブ終了後もプレイヤーの頭上にオハカが乗ってしまうため位置を補正してあげる
            var playerComponentController = playerStateBehaviour.GetComponent<PlayerComponentController>();
            componentController.transform.parent = playerComponentController.DiveObjectTilemapTransform;
            componentController.transform.position -= playerComponentController.transform.position - playerPos;
            playerStateBehaviour = null;
        }
    }
}