using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 扇風機のStateを管理するクラス
    /// </summary>
    public class FanStateMachine : HamuStateMachineBase<FanStateType>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly FanPlayerDivingState playerDivingState;
        /// <summary>
        /// 
        /// </summary>
        private readonly FanNotDivedFallingState notDivedFallingState;
        /// <summary>
        /// 
        /// </summary>
        private readonly FanNotDivedIdlingState notDivedIdlingState;
        /// <summary>
        /// 
        /// </summary>
        private readonly FanNotDivedOnPlayerStayingState notDivedOnPlayerStayingState;
        
        public FanStateMachine(FanObjectComponentController focc)
        {
            playerDivingState = new FanPlayerDivingState(this, focc);
            notDivedFallingState = new FanNotDivedFallingState(this, focc);
            notDivedIdlingState = new FanNotDivedIdlingState(this,focc);
            notDivedOnPlayerStayingState = new FanNotDivedOnPlayerStayingState(this,focc);
        }
        
        protected override IHamuState GetStateFromEnum(FanStateType stateType)
        {
            switch (stateType)
            {
                case FanStateType.PlayerDiving:
                    return playerDivingState;
                case FanStateType.NonDivedFalling:
                    return notDivedFallingState;
                case FanStateType.NonDivedIdling:
                    return notDivedIdlingState;
                case FanStateType.NonDivedOnPlayerStaying:
                    return notDivedOnPlayerStayingState;
                case FanStateType.None:
                    Debug.LogWarning("存在しないSceneModeが渡されました");
                    return null;
                default:
                    Debug.LogWarning("存在しないSceneModeが渡されました");
                    return null;
            }
        }
        
        /// <summary>
        /// ダイブされた際にStateを変更する処理
        /// </summary>
        public void TransitionDiveState()
        {
            ITransitionState<FanStateType> transitionState = this;
            transitionState.TransitionState(FanStateType.PlayerDiving);
        }

        /// <summary>
        /// ダイブが解除されたときにおこなう処理
        /// </summary>
        public void StopDiving()
        {
            ITransitionState<FanStateType> transitionState = this;
            transitionState.TransitionState(FanStateType.NonDivedIdling);
        }
    }
}
