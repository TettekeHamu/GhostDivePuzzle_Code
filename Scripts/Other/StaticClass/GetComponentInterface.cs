using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// インターフェースをGetComponentする用のクラス
    /// </summary>
    public static class GetComponentInterface
    {
        /// <summary>
        /// 指定されたインターフェイスを実装したコンポーネントを持つオブジェクトを探す処理
        /// </summary>
        public static T FindObjectOfInterface<T>() where T : class
        {
            var ns = Object.FindObjectsOfType<Component>();
            var index = 0;
            for (; index < ns.Length; index++)
            {
                var n = ns[index];
                var component = n as T;
                if (component != null)
                {
                    return component;
                }
            }

            return null;
        }
    }
}
