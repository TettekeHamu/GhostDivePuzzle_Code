using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    public class ColiderTest : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D boxCollider2D;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            /*
            var vertices = GetBoxCollider2DVertices.GetBoxCollide2DVertices(boxCollider2D);
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log($"左上のBoxCollider2dの頂点座標は {vertices[0]} です");
                Debug.Log($"右上のBoxCollider2dの頂点座標は {vertices[1]} です");
                Debug.Log($"右下のBoxCollider2dの頂点座標は {vertices[2]} です");
                Debug.Log($"左下のBoxCollider2dの頂点座標は {vertices[3]} です");
            }
            */
        }
    }
}
