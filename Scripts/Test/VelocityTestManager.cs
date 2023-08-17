using System.Collections;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle.Test
{
    public class VelocityTestManager : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb2D;
        
        // Start is called before the first frame update
        void Start()
        {
            //rb2D.velocity = Vector2.right;
            StartCoroutine(VelocityCoroutine());
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private IEnumerator VelocityCoroutine()
        {
            rb2D.velocity = Vector2.right;
            yield return new WaitForSeconds(3);
            rb2D.velocity = Vector2.zero;
        }
    }
}
