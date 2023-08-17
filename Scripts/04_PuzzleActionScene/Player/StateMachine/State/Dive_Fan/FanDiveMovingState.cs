using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 扇風機オブジェクトにダイブ中 & 移動中のState
    /// </summary>
    public class FanDiveMovingState : IHamuState,ICollisionEnemy
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
        /// ダイブ時の機能をまとめたクラス
        /// </summary>
        private readonly PlayerDiveEnterFuncManager playerDiveEnterFuncManager;
        /// <summary>
        /// ダイブ終了時の機能をまとめたクラス
        /// </summary>
        private readonly PlayerDiveExitFuncManager playerDiveExitFuncManager;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public FanDiveMovingState(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
            playerDiveEnterFuncManager = new PlayerDiveEnterFuncManager(ts, pcc);
            playerDiveExitFuncManager = new PlayerDiveExitFuncManager(ts, pcc);
        }
        
        public void Enter()
        {
            
        }

        public void MyUpdate()
        {
            //入力方向に合わせてSpriteの向きを変更(MovingStateと逆になってるので注意！)
            switch (PuzzleActionSceneInputController.Instance.MoveAxisKey.x)
            {
                case > 0:
                    playerComponent.AnimationManager.ChangeSpriteFlipX(true);
                    break;
                case < 0:
                    playerComponent.AnimationManager.ChangeSpriteFlipX(false);
                    break;
            }
            

            //下に地面がなければDivingFallStateに変更
            //var onGround = playerDiveEnterFuncManager.CheckOnGround();
            //if (!onGround)
            {
                transitionState.TransitionState(PlayerStateType.FanDivingJumpDown);
                return;
            }
            
            //移動させる、Rigidbodyで移動させると隙間が通れなくなるので注意
            playerComponent.transform.position +=
                new Vector3(PuzzleActionSceneInputController.Instance.MoveAxisKey.x, 0, 0).normalized * (playerComponent.MoveSpeed / 2 * Time.deltaTime);
            
            //速度が一定以下ならDiveIdleStateに移行する
            if (PuzzleActionSceneInputController.Instance.MoveAxisKey.magnitude < 0.1f)
            {
                transitionState.TransitionState(PlayerStateType.FanDivingIdle);
            }
            //入力があればダイブを解除する
            else if(PuzzleActionSceneInputController.Instance.StopDiveKey)
            {
                transitionState.TransitionState(PlayerStateType.Idle);
                playerDiveExitFuncManager.StopDiving(
                    PlayerDiveType.DiveFan,
                    playerComponent.FanObject.FanObjectComponentController.GroundLayer,
                    playerComponent.FanObject.FanObjectComponentController.ObjectLayer);
            }
            
            if (PuzzleActionSceneInputController.Instance.ActionDiveAbility)
            {
                //ジャンプさせる
                transitionState.TransitionState(PlayerStateType.FanDivingJumpUp);
            }
        }

        public void MyFixedUpdate()
        {

        }

        public void Exit()
        {
            
        }
        
        public void CollisionEnemy()
        {
            transitionState.TransitionState(PlayerStateType.Dead);
        }
    }
}
