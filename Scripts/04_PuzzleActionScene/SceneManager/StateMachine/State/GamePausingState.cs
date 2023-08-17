using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ゲームをポーズ中のState
    /// </summary>
    public class GamePausingState : IHamuState
    {
        /// <summary>
        /// Stateを変更する処理を持つInterface
        /// </summary>
        private readonly ITransitionState<PuzzleActionSceneModeType> transitionState;
        /// <summary>
        /// プレイヤー
        /// </summary>
        private readonly PlayerStateBehaviour playerStateBehaviour;
        /// <summary>
        /// UIを管理するクラス
        /// </summary>
        private readonly UIManager uiManager;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public GamePausingState(ITransitionState<PuzzleActionSceneModeType> ts, PlayerStateBehaviour psb, UIManager uiM)
        {
            transitionState = ts;
            playerStateBehaviour = psb;
            uiManager = uiM;
        }
        
        public void Enter()
        {
            //Debug.Log("一時中止！！");
            //プレイヤーとテキを止める
            playerStateBehaviour.StopMoving();
            //UIを表示
            uiManager.ChangePauseUI(true);
        }

        public void MyUpdate()
        {
            //入力があったらプレイ(GamePlaying)中に戻す
            if (PuzzleActionSceneInputController.Instance.ChangePauseModeKey)
            {
                transitionState.TransitionState(PuzzleActionSceneModeType.GamePlaying);
            }
        }

        public void MyFixedUpdate()
        {
            
        }

        public void Exit()
        {
            //UIを隠す
            uiManager.ChangePauseUI(false);
        }
    }
}
