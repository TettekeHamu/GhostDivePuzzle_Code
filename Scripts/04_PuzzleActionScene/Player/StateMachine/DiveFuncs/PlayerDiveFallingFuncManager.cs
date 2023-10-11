using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using naichilab.EasySoundPlayer.Scripts;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// プレイヤーがダイブ中に接地した際の処理をまとめたクラス
    /// </summary>
    public class PlayerDiveFallingFuncManager
    {
        /// <summary>
        /// Stateを変更させる処理を持つインタフェース
        /// </summary>
        private readonly ITransitionState<PlayerStateType> transitionState;
        /// <summary>
        /// プレイヤーのコンポーネントをまとめたクラス
        /// </summary>
        private readonly PlayerComponentController playerComponent;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public PlayerDiveFallingFuncManager(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
        }

        /// <summary>
        /// 落下を止める処理
        /// </summary>
        /// /// <param name="diveType"></param>
        /// <param name="token"></param>
        public void StopFalling(PlayerDiveType diveType, CancellationToken token)
        {
            AsyncStopFalling(diveType, token).Forget();
            playerComponent.Rigidbody2D.gravityScale = 1;
        }

        /// <summary>
        /// 落下を止める際の非同期処理
        /// </summary>
        /// <param name="diveType"></param>
        /// <param name="token"></param>
        private async UniTaskVoid AsyncStopFalling(PlayerDiveType diveType, CancellationToken token)
        {
            SePlayer.Instance.Play("SE_PlayerLand");
            //パーティクルを再生
            playerComponent.ParticleManager.PlayCollisionGroundParticle();
            //Animatorを変更
            switch (diveType)
            {
                case PlayerDiveType.DiveTomb:
                    playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsFalling,false);
                    break;
                case PlayerDiveType.DiveTV:
                    playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsTVFalling,false);
                    break;
                case PlayerDiveType.DiveFan:
                    playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsFanFalling,false);
                    break;
                case PlayerDiveType.DiveRefrigerator:
                    playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsRefrigeratorFalling,false);
                    break;
                case PlayerDiveType.DiveMicrowave:
                    playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsMicrowaveFalling,false);
                    break;
                default:
                    Debug.LogWarning("そのようなStateは存在しません");
                    break;
            }
            //少し待つ(落下位置を調整するため) & 演出的な不快感をなくすため
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken: token);
            //重力を0に戻す
            playerComponent.Rigidbody2D.gravityScale = 0;
            //Stateを変更
            switch (diveType)
            {
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
                default:
                    Debug.LogWarning("そのようなStateは存在しません");
                    break;
            }
        }
    }
}
