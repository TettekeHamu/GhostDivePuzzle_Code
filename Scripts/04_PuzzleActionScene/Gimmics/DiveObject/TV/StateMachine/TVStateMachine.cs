using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    public class TVStateMachine : HamuStateMachineBase<TVStateType>
    {
        /// <summary>
        /// プレイヤーにダイブされているときのState
        /// </summary>
        private readonly TVPlayerDivingState playerDivingState;
        /// <summary>
        /// ダイブされていない & 落ちているときのState
        /// </summary>
        private readonly TVNotDivedFallingState notDivedFallingState;
        /// <summary>
        /// ダイブされていない & 静止しているときのState
        /// </summary>
        private readonly TVNotDivedIdlingState notDivedIdlingState;
        /// <summary>
        /// ダイブされていない & プレイヤーの上にいるときのState
        /// </summary>
        private readonly TVNotDivedOnPlayerStayingState notDivedOnPlayerStayingState;

        public TVStateMachine(TVObjectComponentController tocc)
        {
            playerDivingState = new TVPlayerDivingState(this, tocc);
            notDivedFallingState = new TVNotDivedFallingState(this, tocc);
            notDivedIdlingState = new TVNotDivedIdlingState(this, tocc);
            notDivedOnPlayerStayingState = new TVNotDivedOnPlayerStayingState(this, tocc);
        }
        
        protected override IHamuState GetStateFromEnum(TVStateType stateType)
        {
            switch (stateType)
            {
                case TVStateType.PlayerDiving:
                    return playerDivingState;
                case TVStateType.NonDivedFalling:
                    return notDivedFallingState;
                case TVStateType.NonDivedIdling:
                    return notDivedIdlingState;
                case TVStateType.NonDivedOnPlayerStaying:
                    return notDivedOnPlayerStayingState;
                case TVStateType.None:
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
            ITransitionState<TVStateType> transitionState = this;
            transitionState.TransitionState(TVStateType.PlayerDiving);
        }

        /// <summary>
        /// ダイブが解除されたときにおこなう処理
        /// </summary>
        public void StopDiving()
        {
            ITransitionState<TVStateType> transitionState = this;
            transitionState.TransitionState(TVStateType.NonDivedIdling);
        }
    }
}
