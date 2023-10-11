using System;
using TettekeKobo.StateMachine;
using UniRx;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 
    /// </summary>
    public class MicrowaveStateMachine : HamuStateMachineBase<MicrowaveStateType>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly MicrowavePlayerDivingState playerDivingState;
        /// <summary>
        /// 
        /// </summary>
        private readonly MicrowaveNotDivedOnPlayerStayingState notDivedOnPlayerStayingState;
        /// <summary>
        /// 
        /// </summary>
        private readonly MicrowaveNotDivedIdlingState notDivedIdlingState;
        /// <summary>
        /// 
        /// </summary>
        private readonly MicrowaveNotDivedFallingState notDivedFallingState;
        /// <summary>
        /// Stateが変更されたときに発行する
        /// </summary>
        private readonly Subject<MicrowaveStateType> onChangeStateSubject = new Subject<MicrowaveStateType>();
        /// <summary>
        /// 
        /// </summary>
        public IObservable<MicrowaveStateType> OnChangeStateObservable => onChangeStateSubject;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public MicrowaveStateMachine(MicrowaveComponentController mcc)
        {
            playerDivingState = new MicrowavePlayerDivingState(this, mcc);
            notDivedOnPlayerStayingState = new MicrowaveNotDivedOnPlayerStayingState(this, mcc);
            notDivedIdlingState = new MicrowaveNotDivedIdlingState(this, mcc);
            notDivedFallingState = new MicrowaveNotDivedFallingState(this, mcc);
        }
        
        protected override IHamuState GetStateFromEnum(MicrowaveStateType stateType)
        {
            //Debug.Log("お墓のStateを変更！！");
            onChangeStateSubject.OnNext(stateType);
            
            switch (stateType)
            {
                case MicrowaveStateType.PlayerDiving:
                    return playerDivingState;
                case MicrowaveStateType.NonDivedFalling:
                    return notDivedFallingState;
                case MicrowaveStateType.NonDivedIdling:
                    return notDivedIdlingState;
                case MicrowaveStateType.NonDivedOnPlayerStaying:
                    return notDivedOnPlayerStayingState;
                case MicrowaveStateType.None:
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
            ITransitionState<MicrowaveStateType> transitionState = this;
            transitionState.TransitionState(MicrowaveStateType.PlayerDiving);
        }

        /// <summary>
        /// ダイブが解除されたときにおこなう処理
        /// </summary>
        public void StopDiving()
        {
            ITransitionState<MicrowaveStateType> transitionState = this;
            transitionState.TransitionState(MicrowaveStateType.NonDivedIdling);
        }
    }
}
