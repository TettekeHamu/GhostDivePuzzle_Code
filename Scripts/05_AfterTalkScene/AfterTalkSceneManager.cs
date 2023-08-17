using Fungus;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// INゲームプレイ後のトークシーンのマネージャークラス
    /// </summary>
    public class AfterTalkSceneManager : MonoBehaviour,ISetUpScene
    {
        /// <summary>
        /// フローチャート
        /// </summary>
        [SerializeField] private Flowchart startFlowchart;
        /// <summary>
        /// ステージのナンバー
        /// </summary>
        private int stageNumber;
        
        public void SetUpScene()
        {
            //ステージナンバーを取得
            stageNumber = PlayStageNumberManager.Instance.LoadStageNumber();
            
            //ステージ数をセーブする
            

            //ナンバーに合わせたトークを開始させる
            if (stageNumber == 0)
            {
                startFlowchart.SendFungusMessage("Tutorial");
            }
            else
            {
                //Debug.Log($"ステージ数は{ stageNumber }です");
                startFlowchart.SendFungusMessage(stageNumber.ToString());
            }
        }


        /// <summary>
        /// Fungus側から呼び出しをおこなう
        /// PuzzleActionSceneに遷移させる処理
        /// </summary>
        public void LoadNextScene()
        {
            SceneLoadManager.Instance.LoadNextScene("StageSelectScene");
        }
    }
}
