using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle.Test
{
    public class Physics2DPlayerManager : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb2D;

        private Vector2 inputVector2;

        private void Start()
        {
            inputVector2 = Vector2.zero;
        }

        private void Update()
        {
            inputVector2 = new Vector2(Input.GetAxis("Horizontal"), 0);
        }

        private void FixedUpdate()
        {
            rb2D.velocity = inputVector2.normalized * 5;
        }
    }
}
