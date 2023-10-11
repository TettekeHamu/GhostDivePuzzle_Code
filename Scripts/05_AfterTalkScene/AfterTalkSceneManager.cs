using System;
using System.Collections;
using Fungus;
using naichilab.EasySoundPlayer.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// INゲームプレイ後のトークシーンのマネージャークラス
    /// </summary>
    public class AfterTalkSceneManager : MonoBehaviour,ISetUpScene
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
            
            //ステージ数をセーブする
            GameDataManager.Instance.UpdateRecentClearStageNumber(stageNumber);
            
            BgmPlayer.Instance.Play("BGM_Talk");

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
        /// 背景を切り替えした後に明転させる処理
        /// </summary>
        public void BrightScene()
        {
            StartCoroutine(BrightSceneCoroutine());
        }
        
        /// <summary>
        /// Fungus側から呼び出しをおこなう
        /// ゆっくり暗転させる処理
        /// </summary>
        public void SleepPlayer()
        {
            StartCoroutine(SleepPlayerCoroutine());
        }

        /// <summary>
        /// Fungus側から呼び出しをおこなう
        /// PuzzleActionSceneに遷移させる処理
        /// </summary>
        public void LoadNextScene()
        {
            BgmPlayer.Instance.Stop();
            if (PlayStageNumberManager.Instance.LoadStageNumber() == 5)
            {
                SceneLoadManager.Instance.LoadNextScene("EndingScene");
            }
            else
            {
                SceneLoadManager.Instance.LoadNextScene("StageSelectScene");   
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        private IEnumerator SleepPlayerCoroutine()
        {
            var startAlpha = 0f;
            while (startAlpha < 1)
            {
                startAlpha += 0.1f;
                var newColor = new Color(0, 0, 0, startAlpha);
                fadeImage.color = newColor;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
