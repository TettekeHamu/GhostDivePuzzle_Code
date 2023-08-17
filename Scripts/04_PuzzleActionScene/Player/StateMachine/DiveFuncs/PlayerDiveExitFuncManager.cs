using Nomnom.RaycastVisualization;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// プレイヤーがダイブ終了時におこなう処理をもつクラス
    /// </summary>
    public class PlayerDiveExitFuncManager 
    {
        /// <summary>
        /// Stateを変更させる処理を持つインタフェース
        /// </summary>
        private readonly ITransitionState<PlayerStateType> transitionState;
        /// <summary>
        /// プレイヤーのコンポーネントをまとめたクラス
        /// </summary>
        private readonly PlayerComponentController playerComponent;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public PlayerDiveExitFuncManager(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
        }
        
        /// <summary>
        /// ダイブをやめる処理
        /// </summary>
        public void StopDiving(PlayerDiveType objectType, LayerMask groundLayer, LayerMask objectLayer)
        {
            var transform = playerComponent.transform;
            Vector2 size = transform.lossyScale;
            Vector2 checkBox = size * 0.85f;
            
            //真上にオブジェクトや地面があるかチェック
            Vector2 upPosition = transform.position + new Vector3(0, size.y, 0);
            var hitUpObject = VisualPhysics2D.OverlapBox(upPosition, checkBox, 0, groundLayer);
            var hitUpGround = VisualPhysics2D.OverlapBox(upPosition, checkBox, 0, objectLayer);
            if(hitUpObject == null && hitUpGround == null)
            {
                transitionState.TransitionState(PlayerStateType.Idle);
                Transform transform1;
                (transform1 = playerComponent.transform).GetChild(0).SetParent(playerComponent.DiveObjectTilemapTransform);
                transform1.position += new Vector3(0, size.y, 0);
                switch (objectType)
                {
                    case PlayerDiveType.DiveTomb:
                        playerComponent.StopDiving();
                        return;
                    case PlayerDiveType.DiveTV:
                        playerComponent.StopTVDiving();
                        return;
                    case PlayerDiveType.DiveFan:
                        playerComponent.StopFanDiving();
                        return;
                    default:
                        Debug.LogWarning("そんなオブジェクトは存在しません");
                        return;
                }
            }

            //左をチェックする
            Vector2 leftPosition = playerComponent.transform.position + new Vector3(size.x * -1f, 0, 0);
            var hitLeftObject = VisualPhysics2D.OverlapBox(leftPosition, checkBox, 0, groundLayer);
            var hitLeftGround = VisualPhysics2D.OverlapBox(leftPosition, checkBox, 0, objectLayer);
            if (hitLeftObject == null && hitLeftGround == null)
            {
                transitionState.TransitionState(PlayerStateType.Idle);
                Transform transform1;
                (transform1 = playerComponent.transform).GetChild(0).SetParent(playerComponent.DiveObjectTilemapTransform);
                transform1.position += new Vector3(size.x * -1, 0, 0);
                switch (objectType)
                {
                    case PlayerDiveType.DiveTomb:
                        playerComponent.StopDiving();
                        return;
                    case PlayerDiveType.DiveTV:
                        playerComponent.StopTVDiving();
                        return;
                    case PlayerDiveType.DiveFan:
                        playerComponent.StopFanDiving();
                        return;
                    default:
                        Debug.LogWarning("そんなオブジェクトは存在しません");
                        return;
                }
            }
            
            //真下をチェック
            Vector2 downPosition = playerComponent.transform.position + new Vector3(0, size.y * -1, 0);
            var hitDownObject = VisualPhysics2D.OverlapBox(downPosition, checkBox, 0, groundLayer);
            var hitDownGround = VisualPhysics2D.OverlapBox(downPosition, checkBox, 0, objectLayer);
            if (hitDownObject == null && hitDownGround == null)
            {
                transitionState.TransitionState(PlayerStateType.Idle);
                Transform transform1;
                (transform1 = playerComponent.transform).GetChild(0).SetParent(playerComponent.DiveObjectTilemapTransform);
                transform1.position += new Vector3(0, size.y * -1, 0);
                switch (objectType)
                {
                    case PlayerDiveType.DiveTomb:
                        playerComponent.StopDiving();
                        return;
                    case PlayerDiveType.DiveTV:
                        playerComponent.StopTVDiving();
                        return;
                    case PlayerDiveType.DiveFan:
                        playerComponent.StopFanDiving();
                        return;
                    default:
                        Debug.LogWarning("そんなオブジェクトは存在しません");
                        return;
                }
            }
            
            //右をチェック
            Vector2 rightPosition = playerComponent.transform.position + new Vector3(size.x, 0, 0);
            var hitRightObject = VisualPhysics2D.OverlapBox(rightPosition, checkBox, 0, groundLayer);
            var hitRightGround = VisualPhysics2D.OverlapBox(rightPosition, checkBox, 0, objectLayer);
            if (hitRightObject == null && hitRightGround == null)
            {
                transitionState.TransitionState(PlayerStateType.Idle);
                Transform transform1;
                (transform1 = playerComponent.transform).GetChild(0).SetParent(playerComponent.DiveObjectTilemapTransform);
                transform1.position += new Vector3(size.x, 0, 0);
                switch (objectType)
                {
                    case PlayerDiveType.DiveTomb:
                        playerComponent.StopDiving();
                        return;
                    case PlayerDiveType.DiveTV:
                        playerComponent.StopTVDiving();
                        return;
                    case PlayerDiveType.DiveFan:
                        playerComponent.StopFanDiving();
                        return;
                    default:
                        Debug.LogWarning("そんなオブジェクトは存在しません");
                        return;
                }
            }
        }
    }
}
