namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// タイトルのシーンタイプを表すEnum
    /// </summary>
    public enum TitleSceneModeType
    {
        /// <summary>
        /// 何もない状態
        /// </summary>
        None = 0,
        /// <summary>
        /// データを読み込み中のState
        /// </summary>
        DataLoading = 1,
        /// <summary>
        /// ゲーム実行中の状態
        /// </summary>
        TitlePlaying = 2,
        /// <summary>
        /// オプションを設定中の状態
        /// </summary>
        OptionSelecting = 3,
        /// <summary>
        /// シーンの切り替えが始まった時の状態
        /// </summary>
        SceneLoading = 4
    };
}
