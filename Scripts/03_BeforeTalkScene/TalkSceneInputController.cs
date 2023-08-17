using System;
using TettekeKobo.Singleton;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// トークシーンの入力を管理するクラス
    /// </summary>
    public class TalkSceneInputController : MonoSingletonBase<TalkSceneInputController>
    {
        /// <summary>
        /// PlayerInputコンポーネント
        /// </summary>
        [SerializeField] private PlayerInput playerInput;
        /// <summary>
        /// 会話を進めるキー入力
        /// </summary>
        private bool advanceDialogueKey;

        public bool AdvanceDialogueKey => advanceDialogueKey;

        private void Update()
        {
            advanceDialogueKey = playerInput.actions["AdvanceDialogue"].WasPressedThisFrame();
        }
    }
}
