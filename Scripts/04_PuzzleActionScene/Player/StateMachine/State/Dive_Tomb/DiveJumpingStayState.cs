using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ジャンプ中に最高地点に到達した際に数フレーム待機させるState
    /// </summary>
    public class DiveJumpingStayState : IHamuState,IDisposable,ICollisionEnemy
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
        /// ダイブ時の機能をまとめたクラス
        /// </summary>
        private readonly PlayerDiveEnterFuncManager playerDiveEnterFuncManager;
        /// <summary>
        /// キャンセル用のトークンソース
        /// </summary>
        private readonly CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public DiveJumpingStayState(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
            playerDiveEnterFuncManager = new PlayerDiveEnterFuncManager(ts, pcc);
            cancellationTokenSource = new CancellationTokenSource();
        }
        
        public void Enter()
        {
            //Debug.Log("最高地点に到達");
            AsyncStopTop(cancellationTokenSource.Token).Forget();
            playerComponent.Rigidbody2D.velocity = Vector2.zero;
        }

        public void MyUpdate()
        {
            
        }

        public void MyFixedUpdate()
        {
            var horizontalVec = PuzzleActionSceneInputController.Instance.MoveAxisKey.x * playerComponent.MoveSpeed / 2f;
            playerComponent.Rigidbody2D.velocity = new Vector2(horizontalVec, 0);
        }

        public void Exit()
        {
            
        }
        
        public void Dispose()
        {
            cancellationTokenSource?.Dispose();
        }

        private async UniTaskVoid AsyncStopTop(CancellationToken token)
        {
            //最高地点で少し待ってから落下させる
            //await UniTask.DelayFrame(1, cancellationToken: token);
            transitionState.TransitionState(PlayerStateType.DivingJumpDown);
        }

        public void CollisionEnemy()
        {
            transitionState.TransitionState(PlayerStateType.Dead);
        }
    }
}
