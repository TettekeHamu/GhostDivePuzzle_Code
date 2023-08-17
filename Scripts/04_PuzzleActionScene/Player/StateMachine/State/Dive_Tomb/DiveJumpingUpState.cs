using System;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// お墓にダイブ中 & ジャンプで上昇中のState
    /// </summary>
    public class DiveJumpingUpState : IHamuState,ICollisionEnemy
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
        /// ジャンプの初速度
        /// </summary>
        private readonly float startJumpVelocity;
        /// <summary>
        /// 目標地点まで達するまでに必要とする時間」
        /// </summary>
        private readonly float requiredTime;
        /// <summary>
        /// 経過時間
        /// </summary>
        private float elapsedTime;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public DiveJumpingUpState(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
            requiredTime = 0.5f;
            var targetHeight = 3;
            startJumpVelocity = 2 * targetHeight / requiredTime;
        }
        
        public void Enter()
        {
            //Debug.Log("ジャンプで上昇中！！");
            elapsedTime = 0;
        }

        public void MyUpdate()
        {
            if (!PuzzleActionSceneInputController.Instance.JumpingUpKey)
            {
                //待機させずに落下させる
                transitionState.TransitionState(PlayerStateType.DivingJumpDown);
            }
        }

        public void MyFixedUpdate()
        {
            // 現在の経過時間に対する割合を出す
            var normalizedTime = elapsedTime / requiredTime;

            // ジャンプ速度を計算
            var currentJumpSpeed = Mathf.Lerp(startJumpVelocity, 0f, normalizedTime);
            
            Debug.Log(currentJumpSpeed);
            
            // Rigidbody2Dにジャンプ速度を設定
            var horizontalVec = PuzzleActionSceneInputController.Instance.MoveAxisKey.x * playerComponent.MoveSpeed / 2f;
            playerComponent.Rigidbody2D.velocity = new Vector2(horizontalVec, currentJumpSpeed);

            // ジャンプ時間を更新
            elapsedTime += Time.fixedDeltaTime;

            // ジャンプが終了したかを確認
            if (elapsedTime >= requiredTime)
            {
                transitionState.TransitionState(PlayerStateType.DivingJumpDown);
            }
        }

        public void Exit()
        {
            
        }

        public void CollisionEnemy()
        {
            transitionState.TransitionState(PlayerStateType.Dead);
        }
    }
}
