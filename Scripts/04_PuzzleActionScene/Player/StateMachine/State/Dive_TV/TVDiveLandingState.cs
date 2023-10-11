using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TettekeKobo.StateMachine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// TVにダイブ中 & 着地した時のState
    /// </summary>
    public class TVDiveLandingState : IHamuState,ICollisionEnemy
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
        private readonly PlayerDiveFallingFuncManager fallingFuncManager;
        /// <summary>
        /// キャンセル用のトークンソース
        /// </summary>
        private readonly CancellationTokenSource cancellationTokenSource;
        
        /// <summary>
        /// コンストラクター
        /// </summary>
        public TVDiveLandingState(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
            fallingFuncManager = new PlayerDiveFallingFuncManager(ts, pcc);
            cancellationTokenSource = new CancellationTokenSource();
        }
        
        public void Enter()
        {
            fallingFuncManager.StopFalling(PlayerDiveType.DiveTV, cancellationTokenSource.Token);
        }

        public void MyUpdate()
        {
            
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
