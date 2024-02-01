using System;
using TettekeKobo.StateMachine;
using UniRx;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// お墓のStateを管理するStateMachine
    /// </summary>
    public class TombObjectStateMachine : HamuStateMachineBase<TombStateType>
    {
        /// <summary>
        /// プレイヤーにダイブされているときのState
        /// </summary>
        private readonly PlayerDivingState playerDivingState;
        /// <summary>
        /// ダイブされていない & 落ちているときのState
        /// </summary>
        private readonly NotDivedFallingState notDivedFallingState;
        /// <summary>
        /// ダイブされていない & 静止しているときのState
        /// </summary>
        private readonly NotDivedIdlingState notDivedIdlingState;
        /// <summary>
        /// ダイブされていない & プレイヤーの上にいるときのState
        /// </summary>
        private readonly NotDivedOnPlayerStayingState notDivedOnPlayerStayingState;
        /// <summary>
        /// Stateが変更されたときに発行する
        /// </summary>
        private readonly Subject<TombStateType> onChangeStateSubject = new Subject<TombStateType>();
        public IObservable<TombStateType> OnChangeStateObservable => onChangeStateSubject;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public TombObjectStateMachine(TombObjectComponentController tocc)
        {
            playerDivingState = new PlayerDivingState(this, tocc);
            notDivedFallingState = new NotDivedFallingState(this, tocc);
            notDivedIdlingState = new NotDivedIdlingState(this, tocc);
            notDivedOnPlayerStayingState = new NotDivedOnPlayerStayingState(this, tocc);
        }
        
        protected override IHamuState GetStateFromEnum(TombStateType stateType)
        {
            //Debug.Log("お墓のStateを変更！！");
            onChangeStateSubject.OnNext(stateType);
            
            switch (stateType)
            {
                case TombStateType.PlayerDiving:
                    return playerDivingState;
                case TombStateType.NonDivedFalling:
                    return notDivedFallingState;
                case TombStateType.NonDivedIdling:
                    return notDivedIdlingState;
                case TombStateType.NonDivedOnPlayerStaying:
                    return notDivedOnPlayerStayingState;
                case TombStateType.None:
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
            ITransitionState<TombStateType> transitionState = this;
            transitionState.TransitionState(TombStateType.PlayerDiving);
        }

        /// <summary>
        /// ダイブが解除されたときにおこなう処理
        /// </summary>
        public void StopDiving()
        {
            ITransitionState<TombStateType> transitionState = this;
            transitionState.TransitionState(TombStateType.NonDivedIdling);
        }
    }
}
