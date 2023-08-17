using System;
using System.Threading;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// タイトルシーンのStateを管理するStateMachine
    /// </summary>
    public class TitleStateMachine : HamuStateMachineBase<TitleSceneModeType>,IDisposable
    {
        /// <summary>
        /// データを読み込み中のState
        /// </summary>
        private readonly DataLoadingState dataLoadingState;
        /// <summary>
        /// ゲーム実行中のState
        /// </summary>
        private readonly TitlePlayingState titlePlayingState;
        /// <summary>
        /// オプション選択中のState
        /// </summary>
        private readonly OptionSelectingState optionSelectingState;
        /// <summary>
        /// シーンロード中のState
        /// </summary>
        private readonly StageSelectSceneLoadingState sceneLoadingState;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public TitleStateMachine(TitleUIController titleUIController)
        {
            //各Stateのインスタンスを生成
            dataLoadingState = new DataLoadingState(this);
            titlePlayingState = new TitlePlayingState(this);
            optionSelectingState = new OptionSelectingState(this, titleUIController);
            sceneLoadingState = new StageSelectSceneLoadingState(this);
        }
        
        /// <summary>
        /// シーンの状態を表すEnumからStateに変換するメソッド
        /// </summary>
        /// <param name="titleSceneModeType">変換前のEnum</param>
        /// <returns>変換したState</returns>
        protected override IHamuState GetStateFromEnum(TitleSceneModeType titleSceneModeType)
        {
            switch (titleSceneModeType)
            {
                case TitleSceneModeType.DataLoading:
                    return dataLoadingState;
                case TitleSceneModeType.TitlePlaying:
                    return titlePlayingState;
                case TitleSceneModeType.OptionSelecting:
                    return optionSelectingState;
                case TitleSceneModeType.SceneLoading:
                    return sceneLoadingState;
                case TitleSceneModeType.None:
                    Debug.LogWarning("存在しないSceneModeが渡されました");
                    return null;
                default:
                    Debug.LogWarning("存在しないSceneModeが渡されました");
                    return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            dataLoadingState?.Dispose();
            optionSelectingState?.Dispose();
        }
    }
}
