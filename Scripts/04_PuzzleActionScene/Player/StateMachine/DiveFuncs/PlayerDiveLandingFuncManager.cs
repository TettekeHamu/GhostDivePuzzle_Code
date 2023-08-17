using System;
using System.Threading;
using Cysharp.Threading.Tasks;
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
        /// <param name="token"></param>
        public void StopFalling(CancellationToken token)
        {
            AsyncStopFalling(token).Forget();
            playerComponent.Rigidbody2D.gravityScale = 1;
        }

        /// <summary>
        /// 落下を止める際の非同期処理
        /// </summary>
        /// <param name="token"></param>
        private async UniTaskVoid AsyncStopFalling(CancellationToken token)
        {
            //パーティクルを再生
            playerComponent.ParticleManager.PlayCollisionGroundParticle();
            //Animatorを変更
            playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsFalling,false);
            //少し待つ(落下位置を調整するため)
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken: token);
            //重力を0に戻す
            playerComponent.Rigidbody2D.gravityScale = 0;
            //Stateを変更
            transitionState.TransitionState(PlayerStateType.DivingIdle);
        }
    }
}
