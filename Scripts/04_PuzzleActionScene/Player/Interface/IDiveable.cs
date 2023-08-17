namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ダイブが可能かどうかを返すメソッドをもつInterface
    /// </summary>
    public interface IDiveable
    {
        /// <summary>
        /// ダイブが可能かどうかを返す処理
        /// </summary>
        /// <returns>ダイブ可能ならtrueを返す</returns>
        public bool GetCanDive();
    }
}
