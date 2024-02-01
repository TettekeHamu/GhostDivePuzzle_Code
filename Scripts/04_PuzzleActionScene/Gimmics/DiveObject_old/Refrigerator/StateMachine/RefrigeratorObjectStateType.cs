namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 冷蔵庫のStateを表すEnum
    /// </summary>
    public enum RefrigeratorObjectStateType
    {
        None = 0,
        PlayerDiving = 1,
        NonDivedFalling = 2,
        NonDivedIdling = 3,
        NonDivedOnPlayerStaying = 4
    };
}
