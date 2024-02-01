using TettekeKobo.Singleton;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// エンドシーンの入力管理クラス
    /// </summary>
    public class EndSceneInputController : MonoSingletonBase<EndSceneInputController>
    {
        /// <summary>
        /// プレイヤーインプットコンポーネント
        /// </summary>
        [SerializeField] private PlayerInput playerInput;
        /// <summary>
        /// シーンを切り替える入力を受け取る値
        /// </summary>
        private bool canChangeScene;
        /// <summary>
        /// オプションを切り替える入力を受け取る値
        /// </summary>
        private bool canChangeOptionMode;
        
        public bool CanChangeScene => canChangeScene;
        
        public bool CanChangeOptionMode => canChangeOptionMode;

        private void Update()
        {
            //入力を受け取る
            canChangeScene = playerInput.actions["ReturnTitle"].WasPressedThisFrame();
        }
    }
}
