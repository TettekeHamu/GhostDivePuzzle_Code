using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using naichilab.EasySoundPlayer.Scripts;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ステージをクリアした時のState
    /// </summary>
    public class GameClearingState : IHamuState,IDisposable
    {
        /// <summary>
        /// Stateを変更させる処理を持つインタフェース
        /// </summary>
        private readonly ITransitionState<PuzzleActionSceneModeType> transitionState;
        /// <summary>
        /// キャンセル用のトークンソース
        /// </summary>
        private readonly CancellationTokenSource cancellationTokenSource;
        
        /// <summary>
        /// コンストラクター
        /// </summary>
        public GameClearingState(ITransitionState<PuzzleActionSceneModeType> ts)
        {
            transitionState = ts;
            cancellationTokenSource = new CancellationTokenSource();
        }
        
        public void Enter()
        {
            SePlayer.Instance.Play("SE_StageClear");
            AsyncLoadNextScene(cancellationTokenSource.Token).Forget();
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

        /// <summary>
        /// 次のシーンを読み込ませる非同期処理
        /// </summary>
        private async UniTaskVoid AsyncLoadNextScene(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);
            transitionState.TransitionState(PuzzleActionSceneModeType.SceneLoading);
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
