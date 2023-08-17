using TettekeKobo.Singleton;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ステージ選択シーンの入力を管理するクラス
    /// </summary>
    public class StageSelectSceneInputController : MonoSingletonBase<StageSelectSceneInputController>
    {
        /// <summary>
        /// PlayerInputコンポーネント
        /// </summary>
        [SerializeField] private PlayerInput playerInput;
        /// <summary>
        /// ステージ数を進める入力のキー（keyDown）
        /// </summary>
        private bool forwardStageNumberKeyDown;
        /// <summary>
        /// ステージ数を進める入力のキー（keyUp）
        /// </summary>
        private bool forwardStageNumberKeyUp;
        /// <summary>
        /// ステージ数を戻す入力のキー（KeyDown）
        /// </summary>
        private bool backwardStageNumberKeyDown;
        /// <summary>
        /// ステージ数を戻す入力のキー（keyUp）
        /// </summary>
        private bool backwardStageNumberKeyUp;
        /// <summary>
        /// ステージ数を決定するキーの入力
        /// </summary>
        private bool decidePlayStageKey;

        public bool ForwardStageNumberKeyDown => forwardStageNumberKeyDown;
        public bool ForwardStageNumberKeyUp => forwardStageNumberKeyUp;
        public bool BackwardStageNumberKeyDown => backwardStageNumberKeyDown;
        public bool BackwardStageNumberKeyUp => backwardStageNumberKeyUp;
        public bool DecidePlayStageKey => decidePlayStageKey;

        private void Update()
        {
            forwardStageNumberKeyDown = playerInput.actions["ForwardStageNumber"].WasPressedThisFrame();
            forwardStageNumberKeyUp = playerInput.actions["ForwardStageNumber"].WasReleasedThisFrame();

            backwardStageNumberKeyDown = playerInput.actions["BackwardStageNumber"].WasPressedThisFrame();
            backwardStageNumberKeyUp = playerInput.actions["BackwardStageNumber"].WasReleasedThisFrame();

            decidePlayStageKey = playerInput.actions["DecidePlayStage"].WasPressedThisFrame();
        }
    }

}
