using System;
using TettekeKobo.Singleton;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TettekeKobo.GhostDivePuzzle
{
    public class ReturnTitleScene : MonoDontDestroySingletonBase<ReturnTitleScene>
    {
        [SerializeField] private PlayerInput playerInput;

        private float timer;

        private void Start()
        {
            timer = 0;
        }

        private void Update()
        {
            var changeScene = playerInput.actions["ReturnTitle"].IsPressed();
            if (changeScene)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
            }

            if (timer > 1f)
            {
                timer = 0;
                SceneLoadManager.Instance.LoadNextScene("TitleScene");
            }
        }
    }
}
