using TettekeKobo.StateMachine;

namespace TettekeKobo.GhostDivePuzzle.Refactoring
{
    /// <summary>
    /// ダイブオブジェクトのStateMachine、継承して使われるためabstractをつける
    /// </summary>
    public abstract class DiveObjectStateMachine : HamuStateMachineBase
    {
        private readonly DiveObjectComponentController docc;

        /// <summary>
        /// 静止中のState
        /// </summary>
        protected IHamuState IdlingState;
        /// <summary>
        /// 落下中のState
        /// </summary>
        protected IHamuState FallingState;
        /// <summary>
        /// プレイヤーの上にいるときのState
        /// </summary>
        protected IHamuState OnPlayerState;
        /// <summary>
        /// プレイヤーにダイブされているときのState
        /// </summary>
        protected IHamuState PlayerDivingState;
        
        /// <summary>
        /// コンストラクター
        /// </summary>
        protected DiveObjectStateMachine()
        {
            
        }

        /// <summary>
        /// ダイブされた際にStateを変更する処理
        /// </summary>
        public void TransitionDiveState()
        {
            ITransitionState transitionState = this;
            transitionState.TransitionState(PlayerDivingState);
        }

        /// <summary>
        /// ダイブが解除されたときにおこなう処理
        /// </summary>
        public void StopDiving()
        {
            ITransitionState transitionState = this;
            transitionState.TransitionState(IdlingState);
        }
    }
}