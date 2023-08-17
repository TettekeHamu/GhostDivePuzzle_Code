using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    public class FanDiveStartingState : IHamuState,IDisposable
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
        public FanDiveStartingState(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
            playerDiveEnterFuncManager = new PlayerDiveEnterFuncManager(ts, pcc);
            cancellationTokenSource = new CancellationTokenSource();
        }
        
        public void Enter()
        {
            //アニメーションの変更
            playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsFanDiving, true);
            //速度をなくす
            playerComponent.Rigidbody2D.velocity = Vector2.zero;
            //Colliderの大きさを変える
            //playerDiveFuncManager.ChangeColliderSize(playerComponent.FanObject.FanObjectComponentController.BoxCollider2D);
            //非同期処理の開始
            AsyncStartDiving(cancellationTokenSource.Token).Forget();
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
        /// ダイブを開始させる非同期処理
        /// </summary>
        private async UniTaskVoid AsyncStartDiving(CancellationToken token)
        {
            //アニメーションの長さを取得
            var length = playerComponent.AnimationManager.GetAnimationLength("Player_DiveStarting_FanAnimation");

            //アニメーションのために当たり判定をなくす
            playerComponent.BoxCollider2D.enabled = false;
            
            //ヒットストップ
            playerComponent.AnimationManager.PlayerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;

            //アニメーションの待機(プレイヤーと敵の座標のXの差分をうまくとる)
            var differencePos = playerComponent.FanObject.transform.position - playerComponent.transform.position;
            await playerComponent.transform.DOMove(new Vector3(differencePos.x / 2, 10, 0), length / 2.0f)
                .SetRelative(true)
                .ToUniTask(cancellationToken: token);

            await playerComponent.transform.DOMove(new Vector3(differencePos.x / 2, -10, 0), length / 2.0f)
                .SetRelative(true)
                .ToUniTask(cancellationToken: token);
            
            //コントローラ＆カメラを振動させる
            PuzzleActionSceneInputController.Instance.VibrationGamePad();
            var source = playerComponent.GetComponent<Cinemachine.CinemachineImpulseSource>();
            source.GenerateImpulse();

            //ヒットストップ終了

            //当たり判定を元に戻す
            playerComponent.BoxCollider2D.enabled = true;
            
            //オハカの座標にPlayerを合わせる
            playerComponent.FanObject.SetPlayerTransform(playerComponent.transform);

            //すぐに入力が入らないように少し待つ
            await UniTask.DelayFrame(10, cancellationToken: token);

            //Stateを変える
            transitionState.TransitionState(PlayerStateType.FanDivingIdle);
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
