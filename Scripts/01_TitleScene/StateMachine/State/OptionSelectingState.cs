using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// オプションを選択中のState
    /// </summary>
    public class OptionSelectingState : IHamuState,IDisposable
    {
        /// <summary>
        /// Stateを遷移させる処理をもつインターフェース
        /// </summary>
        private readonly ITransitionState<TitleSceneModeType> transitionState;
        /// <summary>
        /// UIを管理するクラス
        /// </summary>
        private readonly TitleUIController titleUIController;
        /// <summary>
        /// 非同期処理のキャンセル用のトークンソース
        /// </summary>
        private readonly CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="transitionState">Stateを遷移させる処理をもつインターフェース</param>
        /// <param name="titleUIManager">UI管理用のクラス</param>
        public OptionSelectingState(ITransitionState<TitleSceneModeType> transitionState, TitleUIController titleUIController)
        {
            //インターフェースを受け取る
            this.transitionState = transitionState;
            //UIManagerを格納する
            this.titleUIController = titleUIController;
            cancellationTokenSource = new CancellationTokenSource();
        }

        public void Enter()
        {
            //Debug.Log("オプションを設定してください");
            AsyncOpenOptionUI(cancellationTokenSource.Token).Forget();
        }

        public void MyUpdate()
        {
            //入力があればプレイモードに変更
            if (TitleSceneInputController.Instance.CanChangeOptionMode)
            {
                transitionState.TransitionState(TitleSceneModeType.TitlePlaying);
            }
        }

        public void MyFixedUpdate()
        {
            
        }

        public void Exit()
        {
            //オプション画面を非表示にする
            titleUIController.ChangeOptionUI(false);
        }

        public void Dispose()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
        }
        
        /// <summary>
        /// アニメーション付きでOptionPanelを表示させる処理
        /// </summary>
        /// <param name="token">キャンセルトークン</param>
        private async UniTaskVoid AsyncOpenOptionUI(CancellationToken token)
        {
            //ちょっと待つ
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken: token);
            //表示
            titleUIController.ChangeOptionUI(true);
        }
    }
}
