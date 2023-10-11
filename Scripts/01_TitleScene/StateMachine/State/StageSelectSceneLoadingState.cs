using naichilab.EasySoundPlayer.Scripts;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// シーン遷移中のState
    /// </summary>
    public class StageSelectSceneLoadingState : IHamuState
    {
        /// <summary>
        /// Stateを遷移させる処理をもつインターフェース
        /// </summary>
        private ITransitionState<TitleSceneModeType> transitionState;
        
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="transitionState"></param>
        public StageSelectSceneLoadingState(ITransitionState<TitleSceneModeType> transitionState)
        {
            this.transitionState = transitionState;
        }
        
        public void Enter()
        {
            BgmPlayer.Instance.Stop();
            SceneLoadManager.Instance.LoadNextScene("StageSelectScene");
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
    }
}
