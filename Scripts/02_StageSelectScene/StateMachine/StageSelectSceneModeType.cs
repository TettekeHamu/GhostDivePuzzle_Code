namespace TettekeKobo.GhostDivePuzzle
{
    public enum StageSelectSceneModeType
    {
        None = 0,
        /// <summary>
        /// タイルなどステージ生成中のState
        /// </summary>
        StageCreating = 1,
        /// <summary>
        /// ステージを選択中のState
        /// </summary>
        StageSelecting = 2,
        /// <summary>
        /// 次のシーンを読み込んでいる際のState
        /// </summary>
        SceneLoading = 3
    };
}
