using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TettekeKobo.StateMachine;
using UniRx;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 待機中のState
    /// </summary>
    public class IdlingState : IHamuState,IDiveable,ICollisionEnemy,IStartDiveable,IDisposable
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
        /// ダイブ終了時と同時にダイブさせないように少しだけ待つ用のbool値
        /// </summary>
        private bool isCanDiving;
        /// <summary>
        /// 
        /// </summary>
        private readonly CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public IdlingState(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
            cancellationTokenSource = new CancellationTokenSource();
        }
        
        public void Enter()
        {
            playerComponent.ChangeCanDestroyEnemy(false);
            isCanDiving = false;
            AsyncStartIdling(cancellationTokenSource.Token).Forget();
            //重力を無効化する
            playerComponent.Rigidbody2D.gravityScale = 0;
            //移動をとめる
            playerComponent.Rigidbody2D.velocity = Vector2.zero;
            //移動しやすいようにBoxColliderを少し小さくする
            playerComponent.BoxCollider2D.size = new Vector2(0.85f, 0.85f);
            //アニメーションの変更
            playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsMoving, false);
            playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsDiving, false);
            playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsOffering,false);
            playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsTVDiving,false);
            playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsFanDiving,false);
            playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsRefrigeratorDiving,false);
            playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsMicrowaveDiving,false);
        }

        public void MyUpdate()
        {
            if(!isCanDiving) return;
            
            //ライフゲージを減らす
            var isDead = playerComponent.ReduceLife(Time.deltaTime);
            if (isDead)
            {
                //0以下ならやり直し！！
                transitionState.TransitionState(PlayerStateType.Dead);
            }
            
            //入力が一定以上ならMoveStateに移行
            if (PuzzleActionSceneInputController.Instance.MoveAxisKey.x is > 0.1f or < -0.1f || PuzzleActionSceneInputController.Instance.MoveAxisKey.y is > 0.1f or < -0.1f)
            {
                transitionState.TransitionState(PlayerStateType.Move);
            }
        }

        public void MyFixedUpdate()
        {
            
        }

        public void Exit()
        {
            
        }

        private async UniTaskVoid AsyncStartIdling(CancellationToken token)
        {
            //ダイブ終了時と同時にダイブさせないように少しだけ待つ処理
            await UniTask.DelayFrame(6, cancellationToken: token);
            isCanDiving = true;
        }

        bool IDiveable.GetCanDive()
        {
            if (!isCanDiving) return false;
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
