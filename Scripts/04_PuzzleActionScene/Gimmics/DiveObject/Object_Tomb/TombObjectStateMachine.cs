namespace TettekeKobo.GhostDivePuzzle.Refactoring
{
    /// <summary>
    /// 
    /// </summary>
    public class TombObjectStateMachine : DiveObjectStateMachine
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="docc"></param>
        public TombObjectStateMachine(DiveObjectComponentController docc) : base()
        {
            //各Stateを初期化
            var pState = new PlayerDivingState(docc);
            var opState = new OnPlayerState(this, docc);
            var iState = new IdlingState(this, docc);
            var fState = new FallingState(this, docc);
            //遷移先のStateを渡してあげる
            iState.Initialize(opState,fState);
            fState.Initialize(opState,iState);
            opState.Initialize(iState,fState);
            //StateMachineを初期化
            Initialize(iState);
            //初期化したものを入れる
            IdlingState = iState;
            FallingState = fState;
            OnPlayerState = opState;
            PlayerDivingState = pState;
        }
    }
}