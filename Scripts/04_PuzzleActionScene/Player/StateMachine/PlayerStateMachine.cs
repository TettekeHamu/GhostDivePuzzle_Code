using System;
using TettekeKobo.StateMachine;
using UniRx;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// プレイヤーのState管理を行うStateMachine
    /// </summary>
    public class PlayerStateMachine : HamuStateMachineBase<PlayerStateType>,IDisposable
    {
        /// <summary>
        /// 現在のState（Enum）
        /// </summary>
        private PlayerStateType currentPlayerStateType;
        /// <summary>
        /// プレイヤーが何にダイブしているか
        /// </summary>
        private PlayerDiveType playerDiveType;
        /// <summary>
        /// プレイヤーが死んだらtrueになるbool値
        /// </summary>
        private bool isDeadPlayer;
        /// <summary>
        /// プレイヤーがダイブしているかどうか
        /// </summary>
        private bool isDiving;
        /// <summary>
        /// 待機中のState
        /// </summary>
        private readonly IdlingState idlingState;
        /// <summary>
        /// 移動中のState
        /// </summary>
        private readonly MovingState movingState;
        /// <summary>
        /// 敵と当たったときのState
        /// </summary>
        private readonly DeadState deadState;
        
        /// オハカ関連
        /// <summary>
        /// ダイブを始めた時のState
        /// </summary>
        private readonly DiveStartingState diveStartingState;
        /// <summary>
        /// ダイブ中に待機中のState
        /// </summary>
        private readonly DiveIdlingState diveIdlingState;
        /// <summary>
        /// ダイブ中に移動中のState
        /// </summary>
        private readonly DiveMovingState diveMovingState;
        /// <summary>
        /// ダイブ中に落下しているときのState
        /// </summary>
        private readonly DiveFallingState diveFallingState;
        /// <summary>
        /// ダイブ中に着地した際のState
        /// </summary>
        private readonly DiveLandingState diveLandingState;
        /// <summary>
        /// オソナエが完了したときのState
        /// </summary>
        private readonly DiveOfferingState diveOfferingState;
        /// <summary>
        /// ダイブ中にジャンプで上昇してる際のState
        /// </summary>
        private readonly DiveJumpingUpState diveJumpingUpState;
        /// <summary>
        /// ダイブ中にジャンプで落下してる際のState
        /// </summary>
        private readonly DiveJumpingDownState diveJumpingDownState;

        /// TV関連
        /// <summary>
        /// TVにダイブを始めた時のState
        /// </summary>
        private readonly TVDiveStartingState tvDiveStartingState;
        /// <summary>
        /// TVにダイブ中 & 待機中のState
        /// </summary>
        private readonly TVDiveIdlingState tvDiveIdlingState;
        /// <summary>
        /// TVにダイブ中 & 移動中のState
        /// </summary>
        private readonly TVDiveMovingState tvDiveMovingState;
        /// <summary>
        /// TVにダイブ中 & 落下中のState
        /// </summary>
        private readonly TVDiveFallingState tvDiveFallingState;
        /// <summary>
        /// TVにダイブ中 & 着地した際のState
        /// </summary>
        private readonly TVDiveLandingState tvDiveLandingState;
        /// <summary>
        /// TVにダイブ中 & ジャンプで上昇中のState
        /// </summary>
        private readonly TVDiveJumpingUpState tvDiveJumpingUpState;
        /// <summary>
        /// TVにダイブ中 & ジャンプで下降中のState
        /// </summary>
        private readonly TVDiveJumpingDownState tvDiveJumpingDownState;

        /// 扇風機関連
        /// <summary>
        /// 扇風機にダイブを始めた時のState
        /// </summary>
        private readonly FanDiveStartingState fanDiveStartingState;
        /// <summary>
        /// 扇風機にダイブ中 & 待機中のState
        /// </summary>
        private readonly FanDiveIdlingState fanDiveIdlingState;
        /// <summary>
        /// 扇風機にダイブ中 & 移動中のState
        /// </summary>
        private readonly FanDiveMovingState fanDiveMovingState;
        /// <summary>
        /// 扇風機にダイブ中 & 落下中のState
        /// </summary>
        private readonly FanDiveFallingState fanDiveFallingState;
        /// <summary>
        /// 扇風機にダイブ中 & 着地した際のState
        /// </summary>
        private readonly FanDiveLandingState fanDiveLandingState;
        /// <summary>
        /// 扇風機にダイブ中 & ジャンプ（上昇中）のState
        /// </summary>
        private readonly FanDiveJumpingUpState fanDiveJumpingUpState;
        /// <summary>
        /// 扇風機にダイブ中 & ジャンプ（落下中）のState
        /// </summary>
        private readonly FanDiveJumpingDownState fanDiveJumpingDownState;

        /// 冷蔵庫関連
        /// <summary>
        /// 冷蔵庫にダイブを始めた時のState
        /// </summary>
        private readonly RefrigeratorDiveStartingState refrigeratorDiveStartingState;
        /// <summary>
        /// 冷蔵庫にダイブ中 & 待機中のState
        /// </summary>
        private readonly RefrigeratorDiveIdlingState refrigeratorDiveIdlingState;
        /// <summary>
        /// 冷蔵庫にダイブ中 & 移動中のState
        /// </summary>
        private readonly RefrigeratorDiveMovingState refrigeratorDiveMovingState;
        /// <summary>
        /// 冷蔵庫にダイブ中 & 落下中のState
        /// </summary>
        private readonly RefrigeratorDiveFallingState refrigeratorDiveFallingState;
        /// <summary>
        /// 冷蔵庫にダイブ中 & 着地した際のState
        /// </summary>
        private readonly RefrigeratorDiveLandingState refrigeratorDiveLandingState;
        /// <summary>
        /// 冷蔵庫にダイブ中 & ジャンプで上昇中のState
        /// </summary>
        private readonly RefrigeratorDiveJumpingUpState refrigeratorDiveJumpingUpState;
        /// <summary>
        /// 冷蔵庫にダイブ中 & ジャンプで下降中のState
        /// </summary>
        private readonly RefrigeratorDiveJumpingDownState refrigeratorDiveJumpingDownState;

        /// 電子レンジ関連
        /// <summary>
        /// 
        /// </summary>
        private readonly MicrowaveDiveStartingState microwaveDiveStartingState;
        /// <summary>
        /// 
        /// </summary>
        private readonly MicrowaveDiveIdlingState microwaveDiveIdlingState;
        /// <summary>
        /// 
        /// </summary>
        private readonly MicrowaveDiveMovingState microwaveDiveMovingState;
        /// <summary>
        /// 
        /// </summary>
        private readonly MicrowaveDiveFallingState microwaveDiveFallingState;
        /// <summary>
        /// 
        /// </summary>
        private readonly MicrowaveDiveLandingState microwaveDiveLandingState;
        /// <summary>
        /// 
        /// </summary>
        private readonly MicrowaveDiveJumpingUpState microwaveDiveJumpingUpState;
        /// <summary>
        /// 
        /// </summary>
        private readonly MicrowaveDiveJumpingDownState microwaveDiveJumpingDownState;
        /// <summary>
        /// Stateが変更した時に発行をおこなうSubject
        /// </summary>
        private readonly Subject<PlayerStateType> onChangeStateSubject = new Subject<PlayerStateType>();
        
        ///Getter
        /// <summary>
        /// Stateが変更した時に発行をおこなうSubjectのObservable
        /// </summary>
        public IObservable<PlayerStateType> OnChangeStateObservable => onChangeStateSubject;
        /// <summary>
        /// 外部からプレイヤーが何にダイブしているかを取得する用のgetter
        /// </summary>
        public PlayerDiveType PlayerDiveType => playerDiveType;
        /// <summary>
        /// 
        /// </summary>
        public bool IsDeadPlayer => isDeadPlayer;
        /// <summary>
        /// ダイブしていたらtrueを返すgetter
        /// </summary>
        public bool IsDiving => isDiving;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public PlayerStateMachine(PlayerComponentController pcc)
        {
            isDeadPlayer = false;
            
            idlingState = new IdlingState(this, pcc);
            movingState = new MovingState(this, pcc);
            deadState = new DeadState(this, pcc);
            
            diveStartingState = new DiveStartingState(this, pcc);
            diveIdlingState = new DiveIdlingState(this, pcc);
            diveMovingState = new DiveMovingState(this, pcc);
            diveFallingState = new DiveFallingState(this, pcc);
            diveLandingState = new DiveLandingState(this, pcc);
            diveOfferingState = new DiveOfferingState(this, pcc);
            diveJumpingUpState = new DiveJumpingUpState(this, pcc);
            diveJumpingDownState = new DiveJumpingDownState(this, pcc);

            tvDiveStartingState = new TVDiveStartingState(this,pcc);
            tvDiveIdlingState = new TVDiveIdlingState(this, pcc);
            tvDiveMovingState = new TVDiveMovingState(this, pcc);
            tvDiveFallingState = new TVDiveFallingState(this, pcc);
            tvDiveLandingState = new TVDiveLandingState(this, pcc);
            tvDiveJumpingUpState = new TVDiveJumpingUpState(this, pcc);
            tvDiveJumpingDownState = new TVDiveJumpingDownState(this, pcc);

            fanDiveStartingState = new FanDiveStartingState(this, pcc);
            fanDiveIdlingState = new FanDiveIdlingState(this, pcc);
            fanDiveMovingState = new FanDiveMovingState(this, pcc);
            fanDiveFallingState = new FanDiveFallingState(this, pcc);
            fanDiveLandingState = new FanDiveLandingState(this, pcc);
            fanDiveJumpingUpState = new FanDiveJumpingUpState(this, pcc);
            fanDiveJumpingDownState = new FanDiveJumpingDownState(this, pcc);

            refrigeratorDiveStartingState = new RefrigeratorDiveStartingState(this, pcc);
            refrigeratorDiveIdlingState = new RefrigeratorDiveIdlingState(this, pcc);
            refrigeratorDiveMovingState = new RefrigeratorDiveMovingState(this, pcc);
            refrigeratorDiveFallingState = new RefrigeratorDiveFallingState(this, pcc);
            refrigeratorDiveLandingState = new RefrigeratorDiveLandingState(this, pcc);
            refrigeratorDiveJumpingUpState = new RefrigeratorDiveJumpingUpState(this, pcc);
            refrigeratorDiveJumpingDownState = new RefrigeratorDiveJumpingDownState(this, pcc);

            microwaveDiveStartingState = new MicrowaveDiveStartingState(this, pcc);
            microwaveDiveIdlingState = new MicrowaveDiveIdlingState(this, pcc);
            microwaveDiveMovingState = new MicrowaveDiveMovingState(this, pcc);
            microwaveDiveFallingState = new MicrowaveDiveFallingState(this, pcc);
            microwaveDiveLandingState = new MicrowaveDiveLandingState(this, pcc);
            microwaveDiveJumpingUpState = new MicrowaveDiveJumpingUpState(this, pcc);
            microwaveDiveJumpingDownState = new MicrowaveDiveJumpingDownState(this, pcc);
        }
        
        protected override IHamuState GetStateFromEnum(PlayerStateType stateType)
        {
            onChangeStateSubject.OnNext(stateType);

            currentPlayerStateType = stateType;
            
            switch (stateType)
            {
                //通常状態
                case PlayerStateType.Idle:
                    playerDiveType = PlayerDiveType.NotDive;
                    isDiving = false;
                    return idlingState;
                case PlayerStateType.Move:
                    playerDiveType = PlayerDiveType.NotDive;
                    isDiving = false;
                    return movingState;
                case PlayerStateType.Dead:
                    playerDiveType = PlayerDiveType.NotDive;
                    isDiving = false;
                    isDeadPlayer = true;
                    return deadState;
                //オハカにダイブ
                case PlayerStateType.DivingStart:
                    playerDiveType = PlayerDiveType.DiveTomb;
                    isDiving = true;
                    return diveStartingState;
                case PlayerStateType.DivingIdle:
                    playerDiveType = PlayerDiveType.DiveTomb;
                    isDiving = true;
                    return diveIdlingState;
                case PlayerStateType.DivingMove:
                    playerDiveType = PlayerDiveType.DiveTomb;
                    isDiving = true;
                    return diveMovingState;
                case PlayerStateType.DivingFall:
                    playerDiveType = PlayerDiveType.DiveTomb;
                    isDiving = true;
                    return diveFallingState;
                case PlayerStateType.DivingLand:
                    playerDiveType = PlayerDiveType.DiveTomb;
                    isDiving = true;
                    return diveLandingState;
                case PlayerStateType.DivingOffering:
                    playerDiveType = PlayerDiveType.DiveTomb;
                    isDiving = true;
                    return diveOfferingState;
                case PlayerStateType.DivingJumpUp:
                    playerDiveType = PlayerDiveType.DiveTomb;
                    isDiving = true;
                    return diveJumpingUpState;
                case PlayerStateType.DivingJumpDown:
                    playerDiveType = PlayerDiveType.DiveTomb;
                    isDiving = true;
                    return diveJumpingDownState;
                //TVにダイブ
                case PlayerStateType.TVDivingStart:
                    playerDiveType = PlayerDiveType.DiveTV;
                    isDiving = true;
                    return tvDiveStartingState;
                case PlayerStateType.TVDivingIdle:
                    playerDiveType = PlayerDiveType.DiveTV;
                    isDiving = true;
                    return tvDiveIdlingState;
                case PlayerStateType.TVDivingMove:
                    playerDiveType = PlayerDiveType.DiveTV;
                    isDiving = true;
                    return tvDiveMovingState;
                case PlayerStateType.TVDivingFall:
                    playerDiveType = PlayerDiveType.DiveTV;
                    isDiving = true;
                    return tvDiveFallingState;
                case PlayerStateType.TVDivingLand:
                    playerDiveType = PlayerDiveType.DiveTV;
                    isDiving = true;
                    return tvDiveLandingState;
                case PlayerStateType.TVDivingJumpUp:
                    playerDiveType = PlayerDiveType.DiveTV;
                    isDiving = true;
                    return tvDiveJumpingUpState;
                case PlayerStateType.TVDivingJumpDown:
                    playerDiveType = PlayerDiveType.DiveTV;
                    isDiving = true;
                    return tvDiveJumpingDownState;
                //扇風機にダイブ
                case PlayerStateType.FanDivingStart:
                    playerDiveType = PlayerDiveType.DiveFan;
                    isDiving = true;
                    return fanDiveStartingState;
                case PlayerStateType.FanDivingIdle:
                    playerDiveType = PlayerDiveType.DiveFan;
                    isDiving = true;
                    return fanDiveIdlingState;
                case PlayerStateType.FanDivingMove:
                    playerDiveType = PlayerDiveType.DiveFan;
                    isDiving = true;
                    return fanDiveMovingState;
                case PlayerStateType.FanDivingFall:
                    playerDiveType = PlayerDiveType.DiveFan;
                    isDiving = true;
                    return fanDiveFallingState;
                case PlayerStateType.FanDivingLand:
                    playerDiveType = PlayerDiveType.DiveFan;
                    isDiving = true;
                    return fanDiveLandingState;
                case PlayerStateType.FanDivingJumpUp:
                    playerDiveType = PlayerDiveType.DiveFan;
                    isDiving = true;
                    return fanDiveJumpingUpState;
                case PlayerStateType.FanDivingJumpDown:
                    playerDiveType = PlayerDiveType.DiveFan;
                    isDiving = true;
                    return fanDiveJumpingDownState;
                // 冷蔵庫にダイブ
                case PlayerStateType.RefrigeratorDivingStart:
                    playerDiveType = PlayerDiveType.DiveRefrigerator;
                    isDiving = true;
                    return refrigeratorDiveStartingState;
                case PlayerStateType.RefrigeratorDivingIdle:
                    playerDiveType = PlayerDiveType.DiveRefrigerator;
                    isDiving = true;
                    return refrigeratorDiveIdlingState;
                case PlayerStateType.RefrigeratorDivingMove:
                    playerDiveType = PlayerDiveType.DiveRefrigerator;
                    isDiving = true;
                    return refrigeratorDiveMovingState;
                case PlayerStateType.RefrigeratorDivingFall:
                    playerDiveType = PlayerDiveType.DiveRefrigerator;
                    isDiving = true;
                    return refrigeratorDiveFallingState;
                case PlayerStateType.RefrigeratorDivingLand:
                    playerDiveType = PlayerDiveType.DiveRefrigerator;
                    isDiving = true;
                    return refrigeratorDiveLandingState;
                case PlayerStateType.RefrigeratorDivingJumpUp:
                    playerDiveType = PlayerDiveType.DiveRefrigerator;
                    isDiving = true;
                    return refrigeratorDiveJumpingUpState;
                case PlayerStateType.RefrigeratorDivingJumpDown:
                    playerDiveType = PlayerDiveType.DiveRefrigerator;
                    isDiving = true;
                    return refrigeratorDiveJumpingDownState;
                // 電子レンジにダイブ
                case PlayerStateType.MicrowaveDivingStart:
                    playerDiveType = PlayerDiveType.DiveMicrowave;
                    isDiving = true;
                    return microwaveDiveStartingState;
                case PlayerStateType.MicrowaveDivingIdle:
                    playerDiveType = PlayerDiveType.DiveMicrowave;
                    isDiving = true;
                    return microwaveDiveIdlingState;
                case PlayerStateType.MicrowaveDivingMove:
                    playerDiveType = PlayerDiveType.DiveMicrowave;
                    isDiving = true;
                    return microwaveDiveMovingState;
                case PlayerStateType.MicrowaveDivingFall:
                    playerDiveType = PlayerDiveType.DiveMicrowave;
                    isDiving = true;
                    return microwaveDiveFallingState;
                case PlayerStateType.MicrowaveDivingLand:
                    playerDiveType = PlayerDiveType.DiveMicrowave;
                    isDiving = true;
                    return microwaveDiveLandingState;
                case PlayerStateType.MicrowaveDivingJumpUp:
                    playerDiveType = PlayerDiveType.DiveMicrowave;
                    isDiving = true;
                    return microwaveDiveJumpingUpState;
                case PlayerStateType.MicrowaveDivingJumpDown:
                    playerDiveType = PlayerDiveType.DiveMicrowave;
                    isDiving = true;
                    return microwaveDiveJumpingDownState;
                //その他
                case PlayerStateType.None:
                    Debug.LogWarning("存在しないStateが渡されました");
                    return null;
                default:
                    Debug.LogWarning("存在しないStateが渡されました");
                    return null;
            }
        }

        /// <summary>
        /// ポーズ中やゴール後にプレイヤーを止める用の処理
        /// </summary>
        public void StopPlayer()
        {
            ITransitionState<PlayerStateType> transitionState = this;
            //静止中に変える
            switch (playerDiveType)
            {
                case PlayerDiveType.NotDive:
                    transitionState.TransitionState(PlayerStateType.Idle);
                    break;
                case PlayerDiveType.DiveTomb:
                    transitionState.TransitionState(PlayerStateType.DivingIdle);
                    break;
                case PlayerDiveType.DiveTV:
                    transitionState.TransitionState(PlayerStateType.TVDivingIdle);
                    break;
                case PlayerDiveType.DiveFan:
                    transitionState.TransitionState(PlayerStateType.FanDivingIdle);
                    break;
                case PlayerDiveType.DiveRefrigerator:
                    transitionState.TransitionState(PlayerStateType.RefrigeratorDivingIdle);
                    break;
                case PlayerDiveType.DiveMicrowave:
                    transitionState.TransitionState(PlayerStateType.MicrowaveDivingIdle);
                    break;
                case PlayerDiveType.Non:
                    Debug.LogWarning("存在しないStateが渡されました");
                    break;
                default:
                    Debug.LogWarning("存在しないStateが渡されました");
                    break;
            }
        }

        /// <summary>
        /// 非同期処理のキャンセル用のメソッド
        /// </summary>
        public void Dispose()
        {
            idlingState?.Dispose();
            movingState?.Dispose();
            deadState?.Dispose();
            
            diveStartingState?.Dispose();
            diveMovingState?.Dispose();
            diveFallingState?.Dispose();
            diveLandingState?.Dispose();

            tvDiveStartingState?.Dispose();
            tvDiveMovingState?.Dispose();
            tvDiveFallingState?.Dispose();
            tvDiveLandingState?.Dispose();

            fanDiveStartingState?.Dispose();
            fanDiveMovingState?.Dispose();
            fanDiveFallingState?.Dispose();
            fanDiveLandingState?.Dispose();

            refrigeratorDiveStartingState?.Dispose();
            refrigeratorDiveMovingState?.Dispose();
            refrigeratorDiveFallingState?.Dispose();
            refrigeratorDiveLandingState?.Dispose();
            
            microwaveDiveStartingState?.Dispose();
            microwaveDiveMovingState?.Dispose();
            microwaveDiveFallingState?.Dispose();
            microwaveDiveLandingState?.Dispose();
            
            onChangeStateSubject?.Dispose();
        }
    }
}
