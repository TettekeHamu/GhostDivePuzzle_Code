using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// UIを管理するクラス
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        /// <summary>
        /// キャンバス
        /// </summary>
        [SerializeField] private Canvas canvas;
        /// <summary>
        /// ポーズ中に表示するパネル
        /// </summary>
        [SerializeField] private GameObject pausePanel;
        /// <summary>
        /// 画面右上に出すステージ名のText
        /// </summary>
        [SerializeField] private Text stageNameText;
        /// <summary>
        /// textのPrefab
        /// </summary>
        [SerializeField] private Text textPrefab;
        /// <summary>
        /// ステージ名の開始地点と終了地点
        /// </summary>
        [SerializeField] private Transform[] stageNameTransform;

        private void Awake()
        {
            pausePanel.SetActive(false);
            stageNameText.text = "";
        }

        /// <summary>
        /// ポーズ中に表示するパネルのisActiveを変更する処理
        /// </summary>
        /// <param name="canView">Activeにするかどうか</param>
        public void ChangePauseUI(bool canView)
        {
            pausePanel.SetActive(canView);
        }

        /// <summary>
        /// ステージ名をアニメーション付きで表示するコルーチン
        /// </summary>
        public async UniTask ShowStageNameCoroutine(CancellationToken token)
        {
            var stageName = "";

            if (PlayStageNumberManager.Instance == null)
            {
                Debug.Log("シーン数が不明です");
                return;
            }
            
            switch (PlayStageNumberManager.Instance.LoadStageNumber())
            {
                case 0:
                    stageName = "封木山麓";
                    break;
                case 1:
                    stageName = "封木山山腰";
                    break;
                case 2:
                    stageName = "封木山中腹";
                    break;
                case 3:
                    stageName = "封木山山頂付近";
                    break;
                case 4:
                    stageName = "湯楽神社への道";
                    break;
                case 5:
                    stageName = "湯楽神社入口";
                    break;
                default:
                    stageName = "ステージ名が不明です";
                    break;
            }

            stageNameText.text = stageName;
            
            var textList = new List<Text>();
            await UniTask.Delay((TimeSpan.FromSeconds(0.1f)), cancellationToken: token);
            foreach (var sn in stageName)
            {
                var text = Instantiate(textPrefab, stageNameTransform[0].transform.position, Quaternion.identity);
                textList.Add(text);
                text.transform.SetParent(canvas.transform);
                text.text = sn.ToString();
                StartCoroutine(MoveTextCoroutine(text));
                await UniTask.Delay(TimeSpan.FromSeconds(0.125f), cancellationToken: token);
            }
            
            //とりあえず2秒に設定
            await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: token);

            foreach (var t in textList)
            {
                t.gameObject.SetActive(false);
            }
        }

        private IEnumerator MoveTextCoroutine(Text moveText)
        {
            yield return moveText.rectTransform.DOMove(stageNameTransform[1].transform.position, 0.5f)
                .SetEase(Ease.OutCubic)
                .SetLink(gameObject)
                .WaitForCompletion();
            yield return moveText.rectTransform.DOMove(stageNameTransform[2].transform.position, 1f)
                .SetEase(Ease.Linear)
                .SetLink(gameObject)
                .WaitForCompletion();
            yield return moveText.rectTransform.DOMove(stageNameTransform[3].transform.position, 0.5f)
                .SetEase(Ease.OutCubic)
                .SetLink(gameObject)
                .WaitForCompletion();
        }
    }
}
