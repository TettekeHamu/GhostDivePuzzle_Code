using System;
using TettekeKobo.StateMachine;
using UnityEngine;
using UnityEngine.UI;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ステージ選択シーンのStateMachine
    /// </summary>
    public class StageSelectStateMachine : HamuStateMachineBase<StageSelectSceneModeType>,IDisposable
    {
        /// <summary>
        /// ステージを生成中のState
        /// </summary>
        private readonly StageCreatingState stageCreatingState;
        /// <summary>
        /// ステージを選択中のState
        /// </summary>
        private readonly StageSelectingState stageSelectingState;
        /// <summary>
        /// トークシーンに移行する際のState
        /// </summary>
        private readonly BeforeTalkSceneLoadingState beforeTalkSceneLoadingState;


        /// <summary>
        /// コンストラクター
        /// </summary>
        public StageSelectStateMachine(Text nameText,　TilesManager tilesManager, StageSelectPlayerManager selectPlayerManager)
        {
            stageCreatingState = new StageCreatingState(this, nameText, tilesManager, selectPlayerManager);
            stageSelectingState = new StageSelectingState(this, nameText, tilesManager, selectPlayerManager);
            beforeTalkSceneLoadingState = new BeforeTalkSceneLoadingState(tilesManager);
        }
        
        /// <summary>
        /// シーンの状態を表すEnumからStateに変換するメソッド
        /// </summary>
        /// <param name="stageSelectSceneModeType">変換前のEnum</param>
        /// <returns>変換したState</returns>
        protected override IHamuState GetStateFromEnum(StageSelectSceneModeType stageSelectSceneModeType)
        {
            switch (stageSelectSceneModeType)
            {
                case StageSelectSceneModeType.StageCreating:
                    return stageCreatingState;
                case StageSelectSceneModeType.StageSelecting:
                    return stageSelectingState;
                case StageSelectSceneModeType.SceneLoading:
                    return beforeTalkSceneLoadingState;
                case StageSelectSceneModeType.None:
                    Debug.LogWarning("存在しないSceneModeが渡されました");
                    return null;
                default:
                    Debug.LogWarning("存在しないSceneModeが渡されました");
                    return null;
            }
        }

        public void Dispose()
        {
            stageCreatingState?.Dispose();
        }
    }
}
