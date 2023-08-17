using Fungus;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// INゲームプレイ前のトークシーンのマネージャークラス
    /// </summary>
    public class FungusConnectManager : MonoBehaviour,ISetUpScene
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
            //ナンバーに合わせたパズルアクションシーンを読み込む
            var stageString = "";
            if (stageNumber == 0)
            {
                stageString = "GameScene_" + "Tutorial";
            }
            else
            {
                stageString = "GameScene_" + stageNumber.ToString("D2");
            }

            SceneLoadManager.Instance.LoadNextScene(stageString);
        }
    }
}
