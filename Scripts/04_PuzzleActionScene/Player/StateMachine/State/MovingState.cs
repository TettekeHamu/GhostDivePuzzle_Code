using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using naichilab.EasySoundPlayer.Scripts;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 移動中のState
    /// </summary>
    public class MovingState : IHamuState,IDiveable,ICollisionEnemy,IStartDiveable,IDisposable
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
        /// 経過時間
        /// </summary>
        private float elapsedTime;
        /// <summary>
        /// 
        /// </summary>
        private CancellationTokenSource cancellationTokenSource;

        public MovingState(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
            cancellationTokenSource = new CancellationTokenSource();
        }
        
        private async UniTaskVoid PlayMoveSe(CancellationToken token)
        {
            while (true)
            {
                //Seの長さは0.6秒
                await UniTask.Delay(TimeSpan.FromSeconds(0.9f), cancellationToken: token);
                SePlayer.Instance.Play("SE_PlayerMove");
            }
        }
        
        public void Enter()
        {
            //アニメーション変更
            playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsMoving, true);
            //移動しやすいようにBoxColliderを少し小さくする
            playerComponent.BoxCollider2D.size = new Vector2(0.85f, 0.85f);
            //経過時間をリセット
            elapsedTime = 0;

            cancellationTokenSource = new CancellationTokenSource();
            PlayMoveSe(cancellationTokenSource.Token).Forget();
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
            
            //移動の加速に使う用の時間を加算
            if (elapsedTime <= 1) elapsedTime += Time.deltaTime * 1.1f;
            else elapsedTime = 1;
        }

        public void MyFixedUpdate()
        {
            //入力に合わせて移動させる
            var moveVec = PuzzleActionSceneInputController.Instance.MoveAxisKey.normalized;
            var correctedVec = new Vector2(moveVec.x, moveVec.y / 2);
            playerComponent.Rigidbody2D.velocity = correctedVec * (playerComponent.MoveSpeed * elapsedTime);
        }

        public void Exit()
        {
            cancellationTokenSource.Cancel();
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
        
        void IStartDiveable.StartRefrigeratorDive()
        {
            transitionState.TransitionState(PlayerStateType.RefrigeratorDivingStart);
        }
        
        void IStartDiveable.StartMicrowaveDive()
        {
            transitionState.TransitionState(PlayerStateType.MicrowaveDivingStart);
        }
        
        public void CollisionEnemy()
        {
            transitionState.TransitionState(PlayerStateType.Dead);
        }

        public void Dispose()
        {
            cancellationTokenSource?.Dispose();
        }
    }
}
