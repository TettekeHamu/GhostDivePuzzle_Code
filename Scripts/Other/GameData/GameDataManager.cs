using TettekeKobo.Singleton;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// クリア済みのステージ数などのセーブ＆ロードをおこなうクラス
    /// /// </summary>
    public class GameDataManager : MonoDontDestroySingletonBase<GameDataManager>
    {
        /// <summary>
        /// クリアしたステージの最大のステージ数
        /// </summary>
        private int clearStageNumber;
        /// <summary>
        /// 直近のクリアしたステージのステージ数
        /// </summary>
        private int recentClearStageNumber;
        /// <summary>
        /// 最新のクリアしたステージ数が一番新しいかどうか
        /// </summary>
        private bool isShowNewStage;

        public int ClearStageNumber => clearStageNumber;
        public int RecentClearStageNumber => recentClearStageNumber;
        public bool IsShowNewStage => isShowNewStage;

        /// <summary>
        /// クリアしたステージの最大数を設定する処理（ゲーム起動時に呼びだす）
        /// </summary>
        /// <param name="number"></param>
        public void SetClearStageNumber(int number)
        {
            clearStageNumber = number;
            recentClearStageNumber = 0;
            isShowNewStage = false;
        }

        /// <summary>
        /// ステージクリア時に直近のクリアステージ数を更新する用の処理(ステージクリア時に呼びだす)
        /// </summary>
        /// <param name="number"></param>
        public void UpdateRecentClearStageNumber(int number)
        {
            recentClearStageNumber = number;
            //今までのクリアしたステージ数よりも大きかった場合最大値も更新
            if (recentClearStageNumber > clearStageNumber)
            {
                clearStageNumber = recentClearStageNumber;
                isShowNewStage = true;
            }
            else
            {
                isShowNewStage = false;
            }
        }
    }
}
