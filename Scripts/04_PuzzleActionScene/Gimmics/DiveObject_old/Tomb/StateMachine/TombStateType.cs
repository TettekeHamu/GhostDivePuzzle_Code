namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// オハカのStateを表すEnum
    /// </summary>
    public enum TombStateType
    {
        None = 0,
        PlayerDiving = 1,
        NonDivedFalling = 2,
        NonDivedIdling = 3,
        NonDivedOnPlayerStaying = 4
    };
}
