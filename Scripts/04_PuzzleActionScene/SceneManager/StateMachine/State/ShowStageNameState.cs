using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TettekeKobo.StateMachine;

namespace TettekeKobo.GhostDivePuzzle
{
    public class ShowStageNameState : IHamuState,IDisposable
    {
        /// <summary>
        /// Stateを変更する処理を持つInterface
        /// </summary>
        private readonly ITransitionState<PuzzleActionSceneModeType> transitionState;
        /// <summary>
        /// UIを管理するクラス
        /// </summary>
        private readonly UIManager uiManager;
        /// <summary>
        /// キャンセル用のトークンソース
        /// </summary>
        private readonly CancellationTokenSource cancellationTokenSource;

        public ShowStageNameState(ITransitionState<PuzzleActionSceneModeType> ts, UIManager um)
        {
            transitionState = ts;
            uiManager = um;
            cancellationTokenSource = new CancellationTokenSource();
        }
        
        public void Enter()
        {
            ShowStageName(cancellationTokenSource.Token).Forget();
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
        

        private async UniTaskVoid ShowStageName(CancellationToken token)
        {
            //一文字ずつアニメーションさせる
            await uiManager.ShowStageNameCoroutine(token);
            transitionState.TransitionState(PuzzleActionSceneModeType.GamePlaying);
        }

        public void Dispose()
        {
            cancellationTokenSource?.Dispose();
        }
    }
}
