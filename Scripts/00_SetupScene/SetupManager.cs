using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace TettekeKobo.GhostDivePuzzle
{
    public class SetupManager : MonoBehaviour
    {
        /// <summary>
        /// プレイヤーインプットコンポーネント
        /// </summary>
        [SerializeField] private PlayerInput playerInput;


        private void Awake()
        {
            Application.targetFrameRate = 60;
            SceneManager.LoadScene("TitleScene");
        }

        private void Update()
        {
            //入力を受け取る
            var changeScene = playerInput.actions["GameStart"].WasPressedThisFrame();
            if (changeScene)
            {
                //SceneLoadManager.Instance.LoadNextScene("TitleScene");
            }
        }
    }
}
