using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ステージを生成するState(初期State)
    /// </summary>
    public class StageCreatingState : IHamuState,IDisposable
    {
        /// <summary>
        /// Stateを遷移させる処理をもつインターフェース
        /// </summary>
        private readonly ITransitionState<StageSelectSceneModeType> transitionStageSelectState;
        /// <summary>
        /// タイルを管理する用のコンポーネント
        /// </summary>
        private readonly TilesManager tilesManager;
        /// <summary>
        /// プレイヤーを管理する用のコンポーネント
        /// </summary>
        private readonly StageSelectPlayerManager selectPlayerManager;
        /// <summary>
        /// 非同期処理のキャンセル用のトークンソース
        /// </summary>
        private readonly CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="transitionStageSelectState">Stateを変更する処理を持つInterface</param>
        /// <param name="tilesManager">タイルを生成する用のコンポーネント</param>
        /// <param name="selectPlayerManager">プレイヤーを管理する用のコンポーネント</param>
        public StageCreatingState(ITransitionState<StageSelectSceneModeType> transitionStageSelectState,
                                    TilesManager tilesManager, 
                                    StageSelectPlayerManager selectPlayerManager)
        {
            this.transitionStageSelectState = transitionStageSelectState;
            this.tilesManager = tilesManager;
            this.selectPlayerManager = selectPlayerManager;
            cancellationTokenSource = new CancellationTokenSource();
        }

        public void Enter()
        {
            AsyncCreateStageTile(cancellationTokenSource.Token).Forget();
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
        /// ステージとプレイヤーを生成する処理
        /// </summary>
        /// <param name="token"></param>
        private async UniTaskVoid AsyncCreateStageTile(CancellationToken token)
        {
            //直近のクリアしたステージの番号（なければ0）
            var recentClearStageNumber =　GameDataManager.Instance.RecentClearStageNumber;
            //クリアしたステージの数
            var clearNumber = GameDataManager.Instance.ClearStageNumber;
            //直近のクリアしたステージが最新かどうか
            var isShowNewStage = GameDataManager.Instance.IsShowNewStage;
            
            //クリア済みのステージを表示する
            tilesManager.ShowClearTiles(isShowNewStage, clearNumber);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);

            //直近のクリアしたステージ上にプレイヤーを表示
            selectPlayerManager.ShowPlayer(tilesManager.CanSelectNumberTiles[recentClearStageNumber].transform);
            tilesManager.SetCurrentSelectedTile(recentClearStageNumber);
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f), cancellationToken: token);
            
            //直近のクリアしたステージが一番大きいステージならば新ステージを表示させる
            var isShow = GameDataManager.Instance.IsShowNewStage;
            tilesManager.ShowNewTile(isShow, recentClearStageNumber);
            await UniTask.DelayFrame(1, cancellationToken: token);

            //Stateを変更する
            //Debug.Log($"現在、{tilesManager.NumberTiles.Length}ステージ中、{tilesManager.CanSelectNumberTiles.Count}表示しています");
            transitionStageSelectState.TransitionState(StageSelectSceneModeType.StageSelecting);
        }
    }
}
