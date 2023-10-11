using System.Threading;
using Cysharp.Threading.Tasks;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// TVにダイブ中 & 落下中のState
    /// </summary>
    public class TVDiveFallingState : IHamuState,ICollisionEnemy
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
        /// 
        /// </summary>
        private readonly PlayerDiveUpdateFuncManager updateFuncManager;
        /// <summary>
        /// キャンセル用のトークンソース
        /// </summary>
        private readonly CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public TVDiveFallingState(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
            updateFuncManager = new PlayerDiveUpdateFuncManager(ts, pcc);
            cancellationTokenSource = new CancellationTokenSource();
        }

        public void Enter()
        {
            AsyncStartFalling(cancellationTokenSource.Token).Forget();
        }

        public void MyUpdate()
        {
            var onGround = updateFuncManager.CheckOnGround();
            if (onGround) transitionState.TransitionState(PlayerStateType.TVDivingLand);
        }

        public void MyFixedUpdate()
        {
            
        }

        public void Exit()
        {
            
        }

        /// <summary>
        /// 落下を開始させる処理
        /// </summary>
        private async UniTaskVoid AsyncStartFalling(CancellationToken token)
        {
            //ここで重力をかけることでバグを起こさないようにする
            playerComponent.Rigidbody2D.velocity = Vector2.down;
            
            //少しだけ待つ（ここで待たないとダイブ時にFallingStateに移行してしまうバグが起きる）
            await UniTask.DelayFrame(3, cancellationToken: token);
            
            //接地判定
            var onGround = updateFuncManager.CheckOnGround();
            if (onGround)
            {
                playerComponent.Rigidbody2D.velocity = Vector2.zero;
                //Stateを変更
                transitionState.TransitionState(PlayerStateType.TVDivingIdle);
            }
            else
            {
                //落下させる
                playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsTVFalling, true);
                playerComponent.Rigidbody2D.velocity = Vector2.down * playerComponent.FallSpeed;
            }
        }
        
        public void CollisionEnemy()
        {
            transitionState.TransitionState(PlayerStateType.Dead);
        }

        /// <summary>
        /// 非同期処理のキャンセル用のメソッド
        /// </summary>
        public void Dispose()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
        }
    }
}
