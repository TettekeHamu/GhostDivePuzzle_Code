using System;
using TettekeKobo.Singleton;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// エンドシーンの管理クラス
    /// </summary>
    public class EndSceneBehaviour : MonoBehaviour
    {
        private void Update()
        {
            //シーンを移動
            if (EndSceneInputController.Instance.CanChangeScene)
            {
                SceneLoadManager.Instance.LoadNextScene("TitleScene");
            }
        }
    }
}
