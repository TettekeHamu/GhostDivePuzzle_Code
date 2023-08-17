using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ステージ数の割り当てられたタイルにアタッチするクラス
    /// </summary>
    public class NumberTileManager : MonoBehaviour
    {
        /// <summary>
        /// 割り当てられたステージ数
        /// </summary>
        private int stageNumber;

        public int StageNumber => stageNumber;

        private void Awake()
        {
            //シーン開始時は見えないようにする
            //gameObject.SetActive(false);
        }

        /// <summary>
        /// 初期化用の処理
        /// </summary>
        /// <param name="num">設定したいステージ数</param>
        public void Initialize(int num)
        {
            //ステージ数を設定
            stageNumber = num;
            //可視化する
            gameObject.SetActive(true);
        }
    }
}
