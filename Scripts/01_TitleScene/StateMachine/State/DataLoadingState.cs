using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// セーブデータの読み込みをおこなうState
    /// </summary>
    public class DataLoadingState : IHamuState,IDisposable
    {
        /// <summary>
        /// Stateを遷移させる処理をもつインターフェース
        /// </summary>
        private readonly ITransitionState<TitleSceneModeType> transitionState;
        /// <summary>
        /// 非同期処理のキャンセル用のトークンソース
        /// </summary>
        private readonly CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="transitionState">Stateを遷移させる処理をもつインターフェース</param>
        public DataLoadingState(ITransitionState<TitleSceneModeType> transitionState)
        {
            this.transitionState = transitionState;
            cancellationTokenSource = new CancellationTokenSource();
        }

        public void Enter()
        {
            //Debug.Log("セーブデータを読み込みます");
            AsyncLoadSaveData(cancellationTokenSource.Token).Forget();
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
        
        public void Dispose()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
        }

        /// <summary>
        /// セーブデータを読み込む処理
        /// </summary>
        /// <param name="token">キャンセル用のトークン</param>
        private async UniTaskVoid AsyncLoadSaveData(CancellationToken token)
        {
            await UniTask.DelayFrame(1, cancellationToken: token);
            //ここでセーブデータの読み込みをおこなう(とりあえず4ステージ用意)
            var clearMaxStageNumber = 4;
            //GameDataを管理する方に設定する
            GameDataManager.Instance.SetClearStageNumber(clearMaxStageNumber);
            //Stateを変更する
            transitionState.TransitionState(TitleSceneModeType.TitlePlaying);
        }
    }
}
