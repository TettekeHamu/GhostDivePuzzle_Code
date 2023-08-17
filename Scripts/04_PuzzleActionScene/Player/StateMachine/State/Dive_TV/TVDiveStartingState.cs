using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// TVオブジェクトにダイブを開始した際のState
    /// </summary>
    public class TVDiveStartingState : IHamuState,IDisposable
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
        /// ダイブ開始時の機能をまとめたクラス
        /// </summary>
        private readonly PlayerDiveEnterFuncManager enterFuncManager;
        /// <summary>
        /// キャンセル用のトークンソース
        /// </summary>
        private readonly CancellationTokenSource cancellationTokenSource;
        
        /// <summary>
        /// コンストラクター
        /// </summary>
        public TVDiveStartingState(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
            enterFuncManager = new PlayerDiveEnterFuncManager(ts, pcc);
            cancellationTokenSource = new CancellationTokenSource();
        }
        
        public void Enter()
        {
            enterFuncManager.DiveStart(PlayerDiveType.DiveTV,cancellationTokenSource.Token);
        }

        public void MyUpdate()
        {
            
        }

        public void MyFixedUpdate()
        {
            //移動させないようにする
            playerComponent.Rigidbody2D.velocity = Vector2.zero;
        }

        public void Exit()
        {
            
        }

        /// <summary>
        /// 非同期処理のキャンセル用のメソッド
        /// </summary>
        public void Dispose()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
        }
    }
}
