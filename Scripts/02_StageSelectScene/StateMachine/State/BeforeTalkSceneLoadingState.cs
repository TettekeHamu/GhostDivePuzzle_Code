using naichilab.EasySoundPlayer.Scripts;
using TettekeKobo.StateMachine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// トークシーンに移行するState
    /// </summary>
    public class BeforeTalkSceneLoadingState : IHamuState
    {
        /// <summary>
        /// タイルたちを管理しているクラス
        /// </summary>
        private readonly TilesManager tilesManager;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public BeforeTalkSceneLoadingState(TilesManager tilesManager)
        {
            this.tilesManager = tilesManager;
        }
        
        public void Enter()
        {
            //Debug.Log("トークシーンに移行！！");
            BgmPlayer.Instance.Stop();
            //選択したタイルを設定
            PlayStageNumberManager.Instance.SetStageNumber(tilesManager.CurrentSelectedTile);
            //シーンを遷移
            SceneLoadManager.Instance.LoadNextScene("BeforeTalkScene");
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
