using System.Collections;
using TettekeKobo.Singleton;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// パズルアクションシーンの入力を管理するクラス
    /// </summary>
    public class PuzzleActionSceneInputController : MonoSingletonBase<PuzzleActionSceneInputController>
    {
        /// <summary>
        /// PlayerInputコンポーネント
        /// </summary>
        [SerializeField] private PlayerInput playerInput;
        /// <summary>
        /// ポーズモードに変更を行うキー
        /// </summary>
        private bool changePauseModeKey;
        /// <summary>
        /// 移動に使うキー
        /// </summary>
        private Vector2 moveAxisKey;
        /// <summary>
        /// ジャンプさせるキー
        /// </summary>
        private bool jumpKey;
        /// <summary>
        /// ジャンプ中に押しっぱなしにさせるキー
        /// </summary>
        private bool jumpingUpKey;
        /// <summary>
        /// カメラを切り替える時に使うキー
        /// </summary>
        private bool changeCameraKey;
        /// <summary>
        /// ダイブを開始させるキー
        /// </summary>
        private bool startDiveKey;
        /// <summary>
        /// ダイブを解除するキー
        /// </summary>
        private bool stopDiveKey;
        /// <summary>
        /// ダイブ中の特殊なアクションを実行するキー
        /// </summary>
        private bool actionDiveAbility;
        /// <summary>
        /// リトライ用のキー
        /// </summary>
        private bool retryKey;

        public bool ChangePauseModeKey => changePauseModeKey;
        public Vector2 MoveAxisKey => moveAxisKey;
        public bool JumpKey => jumpKey;
        public bool JumpingUpKey => jumpingUpKey;
        public bool ChangeCameraKey => changeCameraKey;
        public bool StartDiveKey => startDiveKey;
        public bool StopDiveKey => stopDiveKey;
        public bool ActionDiveAbility => actionDiveAbility;
        public bool RetryKey => retryKey;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="waitTime"></param>
        public void VibrationGamePad(float left = 1.0f, float right = 1.0f, float waitTime = 1f)
        {
            var gamepad = Gamepad.current;
            if(gamepad == null) return;
            StartCoroutine(VibrationCoroutine());
        }

        private IEnumerator VibrationCoroutine(float left = 1.0f, float right = 1.0f, float waitTime = 1f)
        {
            var gamepad = Gamepad.current;
            gamepad.SetMotorSpeeds( left, right );
            yield return new WaitForSeconds(waitTime);
            gamepad.SetMotorSpeeds( 0, 0 );
        }

        /// <summary>
        /// 各入力をマイフレーム受け取る
        /// </summary>
        private void Update()
        {
            changePauseModeKey = playerInput.actions["ChangePauseMode"].WasPressedThisFrame();
            moveAxisKey = playerInput.actions["MovePlayer"].ReadValue<Vector2>();
            jumpKey = playerInput.actions["JumpPlayer"].WasPressedThisFrame();
            jumpingUpKey = playerInput.actions["JumpPlayer"].IsPressed();
            changeCameraKey = playerInput.actions["ChangeCamera"].WasPressedThisFrame();
            startDiveKey = playerInput.actions["StartDive"].IsPressed();
            stopDiveKey = playerInput.actions["StopDive"].IsPressed();
            actionDiveAbility = playerInput.actions["ActionDiveAbility"].WasPressedThisFrame();
            retryKey = playerInput.actions["RetryGame"].WasPressedThisFrame();
        }
    }
}
