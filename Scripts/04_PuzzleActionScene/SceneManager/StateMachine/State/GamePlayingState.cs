using System;
using naichilab.EasySoundPlayer.Scripts;
using TettekeKobo.StateMachine;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ゲームをプレイ(再生)中のState
    /// </summary>
    public class GamePlayingState : IHamuState,IDisposable
    {
        /// <summary>
        /// Stateを変更する処理を持つInterface
        /// </summary>
        private readonly ITransitionState<PuzzleActionSceneModeType> transitionState;
        /// <summary>
        /// プレイヤーを管理するクラス
        /// </summary>
        private readonly PlayerStateBehaviour playerStateBehaviour;
        /// <summary>
        /// 敵を管理するクラス
        /// </summary>
        private readonly EnemiesBehaviourController enemiesBehaviourController;
        /// <summary>
        /// ギミックを管理するクラス
        /// </summary>
        private readonly GimmicksBehaviour gimmicksBehaviour;
        /// <summary>
        /// UniRx解除用の変数
        /// </summary>
        private readonly IDisposable dispose;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public GamePlayingState(ITransitionState<PuzzleActionSceneModeType> ts, PlayerStateBehaviour psb, EnemiesBehaviourController ebc,GimmicksBehaviour gb)
        {
            transitionState = ts;
            playerStateBehaviour = psb;
            enemiesBehaviourController = ebc;
            gimmicksBehaviour = gb;
            //ゴールマネジャーのOnGameClearObservableを購読する
            if(gb.GoalObject != null) dispose = gb.GoalObject
                .OnGameClearObservable
                .Subscribe(_ => ChangeGameClearState());
        }
        
        public void Enter()
        {
            BgmPlayer.Instance.Play("BGM_GameScene");
        }

        public void MyUpdate()
        {
            if (PuzzleActionSceneInputController.Instance.RetryKey && !playerStateBehaviour.PlayerStateMachine.IsDeadPlayer)
            {
                //SEを止めるためにプレイヤーをDeadにする
                playerStateBehaviour.PlayerStateMachine.TransitionState(PlayerStateType.Dead);
                return;
            }
            
            /*
            //入力があったらポーズ中に戻す
            if (PuzzleActionSceneInputController.Instance.ChangePauseModeKey)
            {
                transitionState.TransitionState(PuzzleActionSceneModeType.Pausing);
                return;
            }
            //カメラの切り替えをおこなう
            if (PuzzleActionSceneInputController.Instance.ChangeCameraKey)
            {
                CameraManager.Instance.ChangeCamera();
            }
            */
            
            //プレイヤーを動かす
            playerStateBehaviour.MyUpDate();
            //ギミックを動かす
            gimmicksBehaviour.MyUpdate();
            //敵を動かす
            enemiesBehaviourController.MyUpdate();
        }

        public void MyFixedUpdate()
        {
            //プレイヤーを動かす
            playerStateBehaviour.MyFixedUpDate();
            //ギミックを動かす
            gimmicksBehaviour.MyFixedUpDate();
        }

        public void Exit()
        {
            
        }
        
        public void Dispose()
        {
            dispose?.Dispose();
        }
        
        /// <summary>
        /// プレイヤーがゴールと接触した時におこなうメソッド
        /// </summary>
        private void ChangeGameClearState()
        {
            transitionState.TransitionState(PuzzleActionSceneModeType.GameClearing);
        }
    }
}
