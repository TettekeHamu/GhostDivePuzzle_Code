using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ステージ選択シーンのプレイヤーにアタッチするクラス
    /// </summary>
    public class StageSelectPlayerManager : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// プライヤーを可視化する処理
        /// </summary>
        public void ShowPlayer(Transform tileTransform)
        {
            transform.position = tileTransform.position + new Vector3(0, 1, 0);
            gameObject.SetActive(true);
        }
        
        /// <summary>
        /// プレイヤーを動かす処理
        /// </summary>
        public void MovePlayer(Transform tileTransform)
        {
            transform.position = tileTransform.position  + new Vector3(0, 1, 0);
        }
    }
}
