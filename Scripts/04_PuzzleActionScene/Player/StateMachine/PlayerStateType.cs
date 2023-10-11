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

        TVDivingStart = 40,
        TVDivingIdle = 41,
        TVDivingMove = 42,
        TVDivingFall = 43,
        TVDivingLand = 44,
        TVDivingJumpUp = 45,
        TVDivingJumpDown = 46,
        
        FanDivingStart = 50,
        FanDivingIdle = 51,
        FanDivingMove = 52,
        FanDivingFall = 53,
        FanDivingLand = 54,
        FanDivingJumpUp = 55,
        FanDivingJumpDown = 56,

        RefrigeratorDivingStart = 60,
        RefrigeratorDivingIdle = 61,
        RefrigeratorDivingMove = 62,
        RefrigeratorDivingFall = 63,
        RefrigeratorDivingLand = 64,
        RefrigeratorDivingJumpUp = 65,
        RefrigeratorDivingJumpDown = 66,
        
        MicrowaveDivingStart = 70,
        MicrowaveDivingIdle = 71,
        MicrowaveDivingMove = 72,
        MicrowaveDivingFall = 73,
        MicrowaveDivingLand = 74,
        MicrowaveDivingJumpUp = 75,
        MicrowaveDivingJumpDown = 76,

        Dead = 1000
    };

    /// <summary>
    /// プレイヤーのダイブ先を表すEnum
    /// </summary>
    public enum PlayerDiveType
    {
        Non = 0,
        NotDive = 1,
        DiveTomb = 10,
        DiveTV = 20,
        DiveFan = 30,
        DiveRefrigerator = 40,
        DiveMicrowave = 50
    };
}
