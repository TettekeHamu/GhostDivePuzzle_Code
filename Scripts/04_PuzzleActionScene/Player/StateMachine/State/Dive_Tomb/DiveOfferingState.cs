using System.Threading;
using Cysharp.Threading.Tasks;
using naichilab.EasySoundPlayer.Scripts;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// オハカにダイブ中 & オソナエを完了した時のState
    /// </summary>
    public class DiveOfferingState : IHamuState,ICollisionEnemy
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
        /// ダイブ終了時の機能をまとめたクラス
        /// </summary>
        private readonly PlayerDiveExitFuncManager exitFuncManager;
        /// <summary>
        /// 
        /// </summary>
        private readonly CancellationTokenSource cancellationTokenSource;
        
        /// <summary>
        /// コンストラクター
        /// </summary>
        public DiveOfferingState(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
            exitFuncManager = new PlayerDiveExitFuncManager(ts, pcc);
            cancellationTokenSource = new CancellationTokenSource();
        }
        
        public void Enter()
        {
            SePlayer.Instance.Play("SE_PlayerOffer");
            //プレイヤーを停止させる
            playerComponent.Rigidbody2D.velocity = Vector2.zero;
            //オソナエサキの座標に自身を合わせる
            playerComponent.transform.position = playerComponent.TombObject.TombObjectComponent.OfferingManager.NearestOfferingPlace.transform.position;
            //アニメーションの変更
            playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsOffering,true);
            //オソナエできたことをオソナエサキに通知する
            playerComponent.TombObject.TombObjectComponent.OfferingManager.NearestOfferingPlace.SetOfferingObject();
            //ダイブをやめる
            StopDiving(cancellationTokenSource.Token).Forget();
        }

        public void MyUpdate()
        {
            /*
            //入力があればダイブを解除する
            if(PuzzleActionSceneInputController.Instance.StopDiveKey)
            {
                transitionState.TransitionState(PlayerStateType.Idle);
                exitFuncManager.StopDiving(
                    PlayerDiveType.DiveTomb,
                    playerComponent.TombObject.TombObjectComponent.GroundLayer,
                    playerComponent.TombObject.TombObjectComponent.ObjectLayer);
            }
            */
        }

        public void MyFixedUpdate()
        {
            
        }

        public void Exit()
        {
            
        }
        
        public void CollisionEnemy()
        {
            transitionState.TransitionState(PlayerStateType.Dead);
        }

        private async UniTaskVoid StopDiving(CancellationToken token)
        {
            await UniTask.DelayFrame(1, cancellationToken: token);
            transitionState.TransitionState(PlayerStateType.Idle);
            exitFuncManager.StopDiving(
                PlayerDiveType.DiveTomb,
                playerComponent.TombObject.TombObjectComponent.GroundLayer,
                playerComponent.TombObject.TombObjectComponent.ObjectLayer);
        }
    }
}
