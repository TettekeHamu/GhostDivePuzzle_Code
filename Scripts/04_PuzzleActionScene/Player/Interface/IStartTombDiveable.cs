namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ダイブを開始するメソッドを持つInterface
    /// </summary>
    public interface IStartDiveable
    {
        /// <summary>
        /// オハカにダイブを開始する処理
        /// </summary>
        public void StartTombDive();

        /// <summary>
        /// TVにダイブを開始する処理
        /// </summary>
        public void StartTVDive();

        /// <summary>
        /// 扇風機にダイブを開始する処理
        /// </summary>
        public void StartFanDive();

        /// <summary>
        /// 冷蔵庫にダイブを開始する処理
        /// </summary>
        public void StartRefrigeratorDive();

        /// <summary>
        /// 電子レンジにダイブを開始する処理
        /// </summary>
        public void StartMicrowaveDive();
    }
}
