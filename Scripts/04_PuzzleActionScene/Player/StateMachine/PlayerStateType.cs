namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// プレイヤーのStateを表すEnum
    /// </summary>
    public enum PlayerStateType
    {
        None = 0,
        Idle = 10,
        Move = 20,
        
        DivingStart = 30,
        DivingIdle = 31,
        DivingMove = 32,
        DivingFall = 33,
        DivingLand = 34,
        DivingOffering = 35,
        DivingJumpUp = 36,
        DivingJumpDown = 37,
        DiveJumpStay = 38,
        
        TVDivingStart = 40,
        TVDivingIdle = 41,
        TVDivingMove = 42,
        TVDivingFall = 43,
        TVDivingLand = 44,
        
        FanDivingStart = 50,
        FanDivingIdle = 51,
        FanDivingMove = 52,
        FanDivingFall = 53,
        FanDivingLand = 54,
        FanDivingJumpUp = 55,
        FanDivingJumpDown = 56,
        FanDivingJumpIdle = 57,
        
        Dead = 1000
    };

    public enum PlayerDiveType
    {
        Non = 0,
        NotDive = 1,
        DiveTomb = 2,
        DiveTV = 3,
        DiveFan = 4
    };
}
