using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 電子レンジのStateを表すEnum
    /// </summary>
    public enum MicrowaveStateType
    {
        None = 0,
        PlayerDiving = 1,
        NonDivedFalling = 2,
        NonDivedIdling = 3,
        NonDivedOnPlayerStaying = 4
    }
}
