using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 移動中のState
    /// </summary>
    public class MovingState : IHamuState,IDiveable,ICollisionEnemy,IStartDiveable
    {
        /// <summary>
        /// Stateを変更させる処理を持つインタフェース
        /// </summary>
        private readonly ITransitionState<PlayerStateType> transitionState;
        /// <summary>
        /// プレイヤーのコンポーネントをまとめたクラス
        /// </summary>
        private readonly PlayerComponentController playerComponent;

        public MovingState(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
        }
        
        public void Enter()
        {
            //アニメーション変更
            playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsMoving, true);
            //移動しやすいようにBoxColliderを少し小さくする
            playerComponent.BoxCollider2D.size = new Vector2(0.85f, 0.85f);
        }

        public void MyUpdate()
        {
            //ライフゲージを減らす
            var isDead = playerComponent.ReduceLife(Time.deltaTime);
            if (isDead)
            {
                //0以下ならやり直し！！
                transitionState.TransitionState(PlayerStateType.Dead);
            }
            
            //入力方向に合わせてSpriteの向きを変更
            switch (PuzzleActionSceneInputController.Instance.MoveAxisKey.x)
            {
                case > 0:
                    playerComponent.AnimationManager.ChangeSpriteFlipX(false);
                    break;
                case < 0:
                    playerComponent.AnimationManager.ChangeSpriteFlipX(true);
                    break;
            }

            //入力が一定以下ならIdleStateに移行
            if (PuzzleActionSceneInputController.Instance.MoveAxisKey.magnitude < 0.1f)
            {
                transitionState.TransitionState(PlayerStateType.Idle);
            }
        }

        public void MyFixedUpdate()
        {
            //入力に合わせて移動させる
            playerComponent.Rigidbody2D.velocity =
                PuzzleActionSceneInputController.Instance.MoveAxisKey.normalized * playerComponent.MoveSpeed;
        }

        public void Exit()
        {

        }

        bool IDiveable.GetCanDive()
        {
            return PuzzleActionSceneInputController.Instance.StartDiveKey;
        }

        void IStartDiveable.StartTombDive()
        {
            transitionState.TransitionState(PlayerStateType.DivingStart);
        }
        
        void IStartDiveable.StartTVDive()
        {
            transitionState.TransitionState(PlayerStateType.TVDivingStart);
        }

        void IStartDiveable.StartFanDive()
        {
            transitionState.TransitionState(PlayerStateType.FanDivingStart);
        }
        
        public void CollisionEnemy()
        {
            transitionState.TransitionState(PlayerStateType.Dead);
        }
    }
}
