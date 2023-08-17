using System;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    public class PuzzleActionStateMachine : HamuStateMachineBase<PuzzleActionSceneModeType>,IDisposable
    {
        /// <summary>
        /// ゲームをプレイ中（再生中)のState
        /// </summary>
        private readonly GamePlayingState gamePlayingState;
        /// <summary>
        /// ゲームをポーズ中のState
        /// </summary>
        private readonly GamePausingState gamePausingState;
        /// <summary>
        /// ステージをクリアしたときのState
        /// </summary>
        private readonly GameClearingState gameClearingState;
        /// <summary>
        /// ステージクリアした後に会話をおこなうState
        /// </summary>
        private readonly AfterTalkSceneLoadingState afterTalkSceneLoadingState;
        /// <summary>
        /// シーン開始時にステージ名を表示するState
        /// </summary>
        private readonly ShowStageNameState showStageNameState;

        public PuzzleActionStateMachine(PlayerStateBehaviour psb, EnemiesBehaviourController ebc, UIManager uim, GimmicksBehaviour gm)
        {
            gamePlayingState = new GamePlayingState(this, psb, ebc, gm);
            gamePausingState = new GamePausingState(this, psb, uim);
            gameClearingState = new GameClearingState(this);
            afterTalkSceneLoadingState = new AfterTalkSceneLoadingState();
            showStageNameState = new ShowStageNameState(this,uim);
        }
        
        protected override IHamuState GetStateFromEnum(PuzzleActionSceneModeType stateType)
        {
            switch (stateType)
            {
                case PuzzleActionSceneModeType.GamePlaying:
                    return gamePlayingState;
                case PuzzleActionSceneModeType.Pausing:
                    return gamePausingState;
                case PuzzleActionSceneModeType.GameClearing:
                    return gameClearingState;
                case PuzzleActionSceneModeType.SceneLoading:
                    return afterTalkSceneLoadingState;
                case PuzzleActionSceneModeType.ShowStageNumber:
                    return showStageNameState;
                case PuzzleActionSceneModeType.None:
                    Debug.LogWarning("存在しないSceneModeが渡されました");
                    return null;
                default:
                    Debug.LogWarning("存在しないSceneModeが渡されました");
                    return null;
            }
        }

        /// <summary>
        /// 非同期処理のキャンセル用のメソッド
        /// </summary>
        public void Dispose()
        {
            gamePlayingState?.Dispose();
            gameClearingState?.Dispose();
            showStageNameState?.Dispose();
        }
    }
}
