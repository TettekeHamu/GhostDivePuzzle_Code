namespace TettekeKobo.GhostDivePuzzle.Refactoring
{
    /// <summary>
    /// 攻撃可能なオブジェクトのStateMachine
    /// </summary>
    public class AttackableObjectStateMachine : DiveObjectStateMachine
    {
        public AttackableObjectStateMachine(AttackableObjectComponentController aocc) : base()
        {
            //各Stateを初期化
            var pState = new PlayerAttackObjectDivingState(aocc);
            var opState = new OnPlayerState(this, aocc);
            var iState = new IdlingState(this, aocc);
            var fState = new FallingState(this, aocc);
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