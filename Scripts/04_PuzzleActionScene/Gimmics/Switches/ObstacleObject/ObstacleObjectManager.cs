using naichilab.EasySoundPlayer.Scripts;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// おジャマモノにアタッチするクラス
    /// </summary>
    public class ObstacleObjectManager : MonoBehaviour,ISwitchable
    {
        /// <summary>
        /// スイッチがオンかどうか
        /// </summary>
        private bool isActive;

        public bool IsActive => isActive;

        /// <summary>
        /// 初期化用のメソッド
        /// </summary>
        public void Initialize()
        {
            isActive = false;
        }
        
        /// <summary>
        /// スイッチオンになったら消す
        /// </summary>
        public void Activate()
        {
            SePlayer.Instance.Play("SE_SolvePuzzle");
            gameObject.SetActive(false);
        }

        public void Deactivate()
        {
            
        }

        public Vector3 GetObjectPosition()
        {
            return transform.position;
        }
    }
}
