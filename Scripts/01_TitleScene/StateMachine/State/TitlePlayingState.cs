using naichilab.EasySoundPlayer.Scripts;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ゲーム実行中のState
    /// </summary>
    public class TitlePlayingState : IHamuState
    {
        /// <summary>
        /// Stateを遷移させる処理をもつインターフェース
        /// </summary>
        private readonly ITransitionState<TitleSceneModeType> transitionState;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="transitionState">Stateを遷移させる処理をもつインターフェース</param>
        public TitlePlayingState(ITransitionState<TitleSceneModeType> transitionState)
        {
            this.transitionState = transitionState;
        }
        
        public void Enter()
        {
            //Debug.Log("ゲームプレイ中です");
            BgmPlayer.Instance.Stop();
            BgmPlayer.Instance.Play("BGM_Title");
        }

        public void MyUpdate()
        {
            //オプション選択に切り替え
            if (TitleSceneInputController.Instance.CanChangeOptionMode)
            {
                transitionState.TransitionState(TitleSceneModeType.OptionSelecting);
                return;
            }

            //シーンを移動
            if (TitleSceneInputController.Instance.CanChangeScene)
            {
                SePlayer.Instance.Play("SE_Decide");
                transitionState.TransitionState(TitleSceneModeType.SceneLoading);
            }
        }

        public void MyFixedUpdate()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}
