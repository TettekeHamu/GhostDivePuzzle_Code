using System;
using System.Collections;
using Fungus;
using naichilab.EasySoundPlayer.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// INゲームプレイ前のトークシーンのマネージャークラス
    /// </summary>
    public class FungusConnectManager : MonoBehaviour,ISetUpScene
    {
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private Image fadeImage;
        /// <summary>
        /// フローチャート
        /// </summary>
        [SerializeField] private Flowchart startFlowchart;
        /// <summary>
        /// ステージのナンバー
        /// </summary>
        private int stageNumber;

        private void Awake()
        {
            fadeImage.gameObject.SetActive(true);
        }

        public void SetUpScene()
        {
            //ステージナンバーを取得
            stageNumber = PlayStageNumberManager.Instance.LoadStageNumber();
            
            BgmPlayer.Instance.Play("BGM_Talk");

            //ナンバーに合わせたトークを開始させる
            if (stageNumber == 0)
            {
                startFlowchart.SendFungusMessage("Tutorial");
            }
            else
            {
                startFlowchart.SendFungusMessage(stageNumber.ToString());
            }
        }

        /// <summary>
        /// Fungus側から呼び出しをおこなう
        /// 背景を切り替えした後に明転させる処理
        /// </summary>
        public void BrightScene()
        {
            StartCoroutine(BrightSceneCoroutine());
        }


        /// <summary>
        /// Fungus側から呼び出しをおこなう
        /// PuzzleActionSceneに遷移させる処理
        /// </summary>
        public void LoadNextScene()
        {
            BgmPlayer.Instance.Stop();
            //ナンバーに合わせたパズルアクションシーンを読み込む
            var stageString = "";
            if (stageNumber == 0)
            {
                stageString = "TGS_GameScene_" + "Tutorial";
            }
            else
            {
                stageString = "TGS_GameScene_" + stageNumber.ToString("D2");
            }

            SceneLoadManager.Instance.LoadNextScene(stageString);
        }

        private IEnumerator BrightSceneCoroutine()
        {
            var startAlpha = 1f;
            while (startAlpha > 0)
            {
                startAlpha -= 0.1f;
                var newColor = new Color(0, 0, 0, startAlpha);
                fadeImage.color = newColor;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
