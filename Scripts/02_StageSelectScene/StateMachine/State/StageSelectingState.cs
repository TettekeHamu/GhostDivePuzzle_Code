using System;
using TettekeKobo.StateMachine;
using UniRx;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ステージを選択中のState
    /// </summary>
    public class StageSelectingState : IHamuState
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
        private readonly StageSelectPlayerManager playerManager;
        /// <summary>
        /// ステージ数を進める入力を受け付けるObservableのIDisposable
        /// </summary>
        private IDisposable upKeyDisposable;
        /// <summary>
        /// ステージ数を戻す入力を受け付けるObservableのIDisposable
        /// </summary>
        private IDisposable downKeyDisposable;

        public StageSelectingState(ITransitionState<StageSelectSceneModeType> transitionStageSelectState,
                                    TilesManager tilesManager,
                                    StageSelectPlayerManager playerManager)
        {
            this.transitionStageSelectState = transitionStageSelectState;
            this.tilesManager = tilesManager;
            this.playerManager = playerManager;
        }
        
        public void Enter()
        {
            //Debug.Log("ステージを選択してください");
            
            //ステージ数をあげる用の処理
            var numberUpKeyDownStream = Observable.EveryUpdate()
                .Where(_ => StageSelectSceneInputController.Instance.ForwardStageNumberKeyDown);
            var numberUpKeyUpStream = Observable.EveryUpdate()
                .Where(_ => StageSelectSceneInputController.Instance.ForwardStageNumberKeyUp);
            upKeyDisposable = numberUpKeyDownStream
                .Select(_ => Observable.Interval(TimeSpan.FromSeconds(0.5f)).StartWith(0))
                .Switch()
                .TakeUntil(numberUpKeyUpStream)
                .RepeatUntilDestroy(playerManager)
                .Subscribe(_ =>
                {
                    //Debug.Log($"現在の選択中のステージは{tilesManager.CurrentStageNumber}です");
                    //新しいステージ数を取得
                    var newStageNumber = tilesManager.CurrentStageNumber + 1;
                    //早期リターン
                    if (newStageNumber >= tilesManager.CanSelectNumberTiles.Count) return;
                    //ステージ数をあげる
                    //Debug.Log($"新しく選択したステージは{newStageNumber}です");
                    tilesManager.SetCurrentSelectedTile(newStageNumber);
                    //プレイヤーを移動させる
                    playerManager.MovePlayer(tilesManager.CanSelectNumberTiles[newStageNumber].transform);
                });

            //ステージ数をさげる用の処理
            var numberDownKeyDownStream = Observable.EveryUpdate()
                .Where(_ => StageSelectSceneInputController.Instance.BackwardStageNumberKeyDown);
            var numberDownKeyUpStream = Observable.EveryUpdate()
                .Where(_ => StageSelectSceneInputController.Instance.BackwardStageNumberKeyUp);
             downKeyDisposable = numberDownKeyDownStream
                .Select(_ => Observable.Interval(TimeSpan.FromSeconds(0.5f)).StartWith(0))
                .Switch()
                .TakeUntil(numberDownKeyUpStream)
                .RepeatUntilDestroy(playerManager)
                .Subscribe(_ =>
                {
                    //Debug.Log($"現在の選択中のステージは{tilesManager.CurrentStageNumber}です");
                    //新しいステージ数を取得
                    var newStageNumber = tilesManager.CurrentStageNumber - 1;
                    //早期リターン
                    if (newStageNumber < 0) return;
                    //ステージ数をあげる
                    //Debug.Log($"新しく選択したステージは{newStageNumber}です");
                    tilesManager.SetCurrentSelectedTile(newStageNumber);
                    //プレイヤーを移動させる
                    playerManager.MovePlayer(tilesManager.CanSelectNumberTiles[newStageNumber].transform);
                });
        }

        public void MyUpdate()
        {
            //Enterキーを押されたらステージを決定
            if (StageSelectSceneInputController.Instance.DecidePlayStageKey)
            {
                transitionStageSelectState.TransitionState(StageSelectSceneModeType.SceneLoading);
            }
        }

        public void MyFixedUpdate()
        {
            
        }

        public void Exit()
        {
            //入力のObservableを解除する
            upKeyDisposable.Dispose();
            downKeyDisposable.Dispose();
        }
    }
}
