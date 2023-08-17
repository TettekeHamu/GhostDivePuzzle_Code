using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// プレイヤー周りのUI表示を管理するクラス
    /// </summary>
    public class PlayerUIManager : MonoBehaviour
    {
        /// <summary>
        /// プレイヤーのライフゲージ
        /// </summary>
        [SerializeField] private Image playerLifeGauge;
        /// <summary>
        /// プレイヤーのお顔（HPによって変動させる）
        /// </summary>
        [SerializeField] private Image playerFaceImage;
        /// <summary>
        /// プレイヤーの顔の画像をまとめた配列
        /// </summary>
        [SerializeField] private Sprite[] playerFaceImages;

        /// <summary>
        /// ライフゲージを減らす処理
        /// </summary>
        public void ReduceLifeGauge(float cLife, float mLife)
        {
            playerLifeGauge.fillAmount = cLife / mLife;
            ChangePlayerFace(cLife / mLife);
        }

        /// <summary>
        /// ライフゲージを全開させる処理
        /// </summary>
        public void RecoveryLifeGauge(float cLife, float mLife)
        {
            var gaugeTween = DOTween.To(
                () => cLife / mLife,
                x =>
                {
                    playerLifeGauge.fillAmount = x;
                },
                1,
                0.5f
            );
            ChangePlayerFace(1);
        }

        /// <summary>
        /// プレイヤーの顔を変更させる処理
        /// </summary>
        /// <param name="rate"></param>
        private void ChangePlayerFace(float rate)
        {
            if (rate >= 0.8f)
            {
                playerFaceImage.sprite = playerFaceImages[0];
            }
            else if(rate >= 0.3f)
            {
                playerFaceImage.sprite = playerFaceImages[1];
            }
            else
            {
                playerFaceImage.sprite = playerFaceImages[2];
            }
        }
    }
}
