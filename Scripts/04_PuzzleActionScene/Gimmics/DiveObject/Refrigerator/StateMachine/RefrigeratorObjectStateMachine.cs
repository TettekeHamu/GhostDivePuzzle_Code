using System;
using TettekeKobo.StateMachine;
using UniRx;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ExampleObjectStateMachine
    /// </summary>
    public class RefrigeratorObjectStateMachine : HamuStateMachineBase<RefrigeratorObjectStateType>
    {
        /// <summary>
        /// プレイヤーにダイブされているときのState
        /// </summary>
        private readonly RefrigeratorObjectPlayerDivingState playerDivingState;
        /// <summary>
        /// ダイブされていない & 落ちているときのState
        /// </summary>
        private readonly RefrigeratorObjectNotDivedFallingState notDivedFallingState;
        /// <summary>
        /// ダイブされていない & 静止しているときのState
        /// </summary>
        private readonly RefrigeratorObjectNotDivedIdlingState notDivedIdlingState;
        /// <summary>
        /// ダイブされていない & プレイヤーの上にいるときのState
        /// </summary>
        private readonly RefrigeratorObjectNotDivedOnPlayerState notDivedOnPlayerStayingState;
        /// <summary>
        /// Stateが変更されたときに発行する
        /// </summary>
        private readonly Subject<RefrigeratorObjectStateType> onChangeStateSubject = new Subject<RefrigeratorObjectStateType>();
        /// <summary>
        /// 
        /// </summary>
        public IObservable<RefrigeratorObjectStateType> OnChangeStateObservable => onChangeStateSubject;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public RefrigeratorObjectStateMachine(RefrigeratorObjectComponentController rocc)
        {
            playerDivingState = new RefrigeratorObjectPlayerDivingState(this, rocc);
            notDivedFallingState = new RefrigeratorObjectNotDivedFallingState(this, rocc);
            notDivedIdlingState = new RefrigeratorObjectNotDivedIdlingState(this, rocc);
            notDivedOnPlayerStayingState = new RefrigeratorObjectNotDivedOnPlayerState(this, rocc);
        }
        
        protected override IHamuState GetStateFromEnum(RefrigeratorObjectStateType stateType)
        {
            //Debug.Log("お墓のStateを変更！！");
            onChangeStateSubject.OnNext(stateType);
            
            switch (stateType)
            {
                case RefrigeratorObjectStateType.PlayerDiving:
                    return playerDivingState;
                case RefrigeratorObjectStateType.NonDivedFalling:
                    return notDivedFallingState;
                case RefrigeratorObjectStateType.NonDivedIdling:
                    return notDivedIdlingState;
                case RefrigeratorObjectStateType.NonDivedOnPlayerStaying:
                    return notDivedOnPlayerStayingState;
                case RefrigeratorObjectStateType.None:
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
            ITransitionState<RefrigeratorObjectStateType> transitionState = this;
            transitionState.TransitionState(RefrigeratorObjectStateType.PlayerDiving);
        }

        /// <summary>
        /// ダイブが解除されたときにおこなう処理
        /// </summary>
        public void StopDiving()
        {
            ITransitionState<RefrigeratorObjectStateType> transitionState = this;
            transitionState.TransitionState(RefrigeratorObjectStateType.NonDivedIdling);
        }
    }
}
