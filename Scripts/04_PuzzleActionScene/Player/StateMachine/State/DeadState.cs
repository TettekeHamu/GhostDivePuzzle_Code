using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using naichilab.EasySoundPlayer.Scripts;
using TettekeKobo.StateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 死んでいるときのState
    /// </summary>
    public class DeadState : IHamuState,IDisposable
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
        /// キャンセル用のトークンソース
        /// </summary>
        private readonly CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public DeadState(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
            cancellationTokenSource = new CancellationTokenSource();
        }
        
        public void Enter()
        {
            SePlayer.Instance.Play("SE_PlayerDead");
            AsyncDeadPlayer(cancellationTokenSource.Token).Forget();
        }

        public void MyUpdate()
        {
            
        }

        public void MyFixedUpdate()
        {
            
        }

        public void Exit()
        {
            
        }

        private async UniTaskVoid AsyncDeadPlayer(CancellationToken token)
        {
            playerComponent.SpriteRenderer.enabled = false;
            playerComponent.Rigidbody2D.velocity = Vector2.zero;
            var time = playerComponent.ParticleManager.PlayDeadParticle();
            await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: token);
            //Debug.Log("やり直し");
            //シーンの再読み込みをおこなう
            SceneLoadManager.Instance.LoadNextScene(SceneManager.GetActiveScene().name);
        }

        public void Dispose()
        {
            cancellationTokenSource?.Dispose();
        }
    }
}
