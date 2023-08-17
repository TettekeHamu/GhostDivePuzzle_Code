using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// スイッチの機能を提供するInterface
    /// </summary>
    public interface ISwitchable
    {
        /// <summary>
        /// スイッチがオンかどうか
        /// </summary>
        public bool IsActive { get; }
        
        /// <summary>
        /// スイッチをオンにする際の処理
        /// </summary>
        public void Activate();
        
        /// <summary>
        /// スイッチをオフにする際の処理
        /// </summary>
        public void Deactivate();

        /// <summary>
        /// 実装したオブジェクトの位置を返すメソッド
        /// </summary>
        /// <returns>座標</returns>
        public Vector3 GetObjectPosition();
    }
}
