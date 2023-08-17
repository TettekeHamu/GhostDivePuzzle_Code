using UnityEngine;
using UnityEngine.UI;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// タイトルシーンのUI管理をおこなうクラス
    /// </summary>
    public class TitleUIController : MonoBehaviour
    {
        /// <summary>
        /// オプション選択の画面（仮）
        /// </summary>
        [SerializeField] private Image optionImage;

        /// <summary>
        /// 初期化用処理
        /// </summary>
        public void Initialize()
        {
            optionImage.gameObject.SetActive(false);
        }
        
        /// <summary>
        /// オプションのUIの表示・非表示をおこなうクラス
        /// </summary>
        /// <param name="canView">trueなら可視化させる</param>
        public void ChangeOptionUI(bool canView)
        {
            optionImage.gameObject.SetActive(canView);
        }
    }
}
