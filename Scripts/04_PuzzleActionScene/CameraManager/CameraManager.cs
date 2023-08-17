using Cinemachine;
using TettekeKobo.Singleton;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// カメラの切り替えをおこなうクラス
    /// </summary>
    public class CameraManager : MonoSingletonBase<CameraManager>
    {
        /// <summary>
        /// 広く見渡す用のカメラ
        /// </summary>
        [SerializeField] private CinemachineVirtualCamera closeCamera;

        protected override void Awake()
        {
            base.Awake();
            closeCamera.Priority = 1;
        }

        /// <summary>
        /// カメラを切り替えるメソッド、Game全体のStateMachineが呼びだす
        /// </summary>
        public void ChangeCamera()
        {
            closeCamera.Priority *= -1;
        }
    }
}
