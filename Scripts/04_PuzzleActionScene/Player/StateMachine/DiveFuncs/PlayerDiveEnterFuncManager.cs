using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using naichilab.EasySoundPlayer.Scripts;
using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// プレイヤーがダイブ開始時におこなう処理をもつクラス
    /// </summary>
    public class PlayerDiveEnterFuncManager
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
        public PlayerDiveEnterFuncManager(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
        }

        /// <summary>
        /// ダイブ開始時の処理をまとめたメソッド
        /// </summary>
        /// <param name="diveType">ダイブの種類（オハカとかTVとか）</param>
        /// <param name="token"></param>
        public void DiveStart(PlayerDiveType diveType, CancellationToken token)
        {
            //ダイブ先の種類に応じてアニメーションを変える
            switch (diveType)
            {
                case PlayerDiveType.DiveTomb:
                    playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsDiving,true);
                    break;
                case PlayerDiveType.DiveTV:
                    playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsTVDiving,true);
                    break;
                case PlayerDiveType.DiveFan:
                    playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsFanDiving,true);
                    break;
                case PlayerDiveType.DiveRefrigerator:
                    playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsRefrigeratorDiving,true);
                    break;
                case PlayerDiveType.DiveMicrowave:
                    playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsMicrowaveDiving,true);
                    break;
                default:
                    Debug.LogWarning("ダイブ先が不明です");
                    break;
            }
            //速度をなくす
            playerComponent.Rigidbody2D.velocity = Vector2.zero;
            //Colliderの大きさを変える
            playerComponent.BoxCollider2D.size = new Vector2(0.95f, 0.95f);
            //ダイブを開始させる非同期処理の開始
            AsyncStartDiving(diveType, token).Forget();
        }

        /// <summary>
        /// ダイブ中のアニメーションをおこなう非同期処理
        /// </summary>
        /// <param name="diveType"></param>
        /// <param name="token"></param>
        private async UniTaskVoid AsyncStartDiving(PlayerDiveType diveType, CancellationToken token)
        {
            //アニメーションの長さを取得
            var length = 0f;
            switch (diveType)
            {
                case PlayerDiveType.DiveTomb:
                    length = playerComponent.AnimationManager.GetAnimationLength("Player_DiveStartingAnimation");
                    break;
                case PlayerDiveType.DiveTV:
                    length = playerComponent.AnimationManager.GetAnimationLength("Player_DiveStarting_TVAnimation");
                    break;
                case PlayerDiveType.DiveFan:
                    length = playerComponent.AnimationManager.GetAnimationLength("Player_DiveStarting_FanAnimation");
                    break;
                case PlayerDiveType.DiveRefrigerator:
                    length = playerComponent.AnimationManager.GetAnimationLength("Player_DiveStarting_RefrigeratorAnimation");
                    break;
                case PlayerDiveType.DiveMicrowave:
                    length = playerComponent.AnimationManager.GetAnimationLength("Player_DiveStarting_MicrowaveAnimation");
                    break;
                default:
                    Debug.LogWarning("ダイブ先が不明です");
                    break;
            }

            //アニメーションのために当たり判定をなくす
            playerComponent.BoxCollider2D.enabled = false;
            
            //座標の差分を取ってアニメーションさせる
            var differencePos = Vector3.zero;
            switch (diveType)
            {
                case PlayerDiveType.DiveTomb:
                    differencePos = playerComponent.TombObject.transform.position - playerComponent.transform.position;
                    break;
                case PlayerDiveType.DiveTV:
                    differencePos = playerComponent.TVObject.transform.position - playerComponent.transform.position;
                    break;
                case PlayerDiveType.DiveFan:
                    differencePos = playerComponent.FanObject.transform.position - playerComponent.transform.position;
                    break;
                case PlayerDiveType.DiveRefrigerator:
                    differencePos = playerComponent.RefrigeratorObject.transform.position - playerComponent.transform.position;
                    break;
                case PlayerDiveType.DiveMicrowave:
                    differencePos = playerComponent.MicrowaveObject.transform.position - playerComponent.transform.position;
                    break;
                default:
                    Debug.LogWarning("ダイブ先が不明です");
                    break;
            }
            await playerComponent.transform.DOMove(new Vector3(differencePos.x / 2, 4, 0), length / 2.0f)
                .SetRelative(true)
                .ToUniTask(cancellationToken: token);

            await playerComponent.transform.DOMove(new Vector3(differencePos.x / 2, -4, 0), length / 2.0f)
                .SetRelative(true)
                .ToUniTask(cancellationToken: token);
            
            SePlayer.Instance.Play("SE_PlayerDive");
            
            //コントローラを振動させる
            PuzzleActionSceneInputController.Instance.VibrationGamePad();
            var source = playerComponent.GetComponent<Cinemachine.CinemachineImpulseSource>();
            source.GenerateImpulse();

            //当たり判定を元に戻す
            playerComponent.BoxCollider2D.enabled = true;
            
            //ダイブ先の座標にPlayerを合わせる
            switch (diveType)
            {
                case PlayerDiveType.DiveTomb:
                    playerComponent.TombObject.SetPlayerTransform(playerComponent.transform);
                    break;
                case PlayerDiveType.DiveTV:
                    playerComponent.TVObject.SetPlayerTransform(playerComponent.transform);
                    break;
                case PlayerDiveType.DiveFan:
                    playerComponent.FanObject.SetPlayerTransform(playerComponent.transform);
                    break;
                case PlayerDiveType.DiveRefrigerator:
                    playerComponent.RefrigeratorObject.SetPlayerTransform(playerComponent.transform);
                    break;
                case PlayerDiveType.DiveMicrowave:
                    playerComponent.MicrowaveObject.SetPlayerTransform(playerComponent.transform);
                    break;
                default:
                    Debug.LogWarning("ダイブ先が不明です");
                    break;
            }

            //ライフを元に戻す
            playerComponent.RecoverLife();
            
            //DiveLandingStateにならないように重力をかける
            playerComponent.Rigidbody2D.gravityScale = 1;

            //すぐに入力が入らないように少し待つ（ここで接地させる）
            await UniTask.DelayFrame(10, cancellationToken: token);

            //重力を0に直す
            playerComponent.Rigidbody2D.gravityScale = 0;

            //Stateを変える
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
                    Debug.LogWarning("ダイブ先が不明です");
                    break;
            }
        }
    }
}
