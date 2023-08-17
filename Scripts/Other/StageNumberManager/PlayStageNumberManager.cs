using TettekeKobo.Singleton;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// プレイするステージ数を格納するコンポーネント
    /// </summary>
    public class PlayStageNumberManager : MonoDontDestroySingletonBase<PlayStageNumberManager>
    {
        /// <summary>
        /// プレイ中のステージ番号
        /// </summary>
        private int currentStageNumber;

        /// <summary>
        /// ステージ数を設定する処理
        /// </summary>
        /// <param name="tileManager">ステージ数</param>
        public void SetStageNumber(NumberTileManager tileManager)
        {
            currentStageNumber = tileManager.StageNumber;
        }

        /// <summary>
        /// ステージ数を取得する処理
        /// </summary>
        /// <returns></returns>
        public int LoadStageNumber()
        {
            return currentStageNumber;
        }
    }
}
