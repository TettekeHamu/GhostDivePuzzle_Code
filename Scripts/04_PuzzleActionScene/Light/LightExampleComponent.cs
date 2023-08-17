using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ライトの説明をおこなうコンポーネント
    /// </summary>
    public class LightExampleComponent : MonoBehaviour
    {
        [SerializeField, MultilineAttribute (5)] private string exampleText;
    }
}
