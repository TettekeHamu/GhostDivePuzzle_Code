using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle.Test
{
    public class JumpTest : MonoBehaviour
    {
        public float jumpDistance = 5f;         // Inspectorからジャンプ距離を設定
        public float timeToReachJumpPoint = 1f; // Inspectorからジャンプまでの時間を設定

        private Rigidbody2D rb;
        private float initialJumpSpeed;
        private float jumpTimer;
        private bool isJumping;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            isJumping = false;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                StartJump();
            }
        }

        void FixedUpdate()
        {
            if (isJumping)
            {
                // 現在の経過時間に対する補完率を計算
                float normalizedTime = jumpTimer / timeToReachJumpPoint;

                // 補完率を元にジャンプ速度を計算
                float currentJumpSpeed = Mathf.Lerp(initialJumpSpeed, 0f, normalizedTime);
                
                Debug.Log(rb.velocity.y);

                // Rigidbody2Dにジャンプ速度を設定
                rb.velocity = new Vector2(rb.velocity.x, currentJumpSpeed);

                // ジャンプ時間を更新
                jumpTimer += Time.fixedDeltaTime;

                // ジャンプが終了したかを確認
                if (jumpTimer >= timeToReachJumpPoint)
                {
                    isJumping = false;
                }
            }
        }

        void StartJump()
        {
            // 現在の速度を保持し、初期ジャンプ速度を計算
            initialJumpSpeed = 2f * jumpDistance / timeToReachJumpPoint;
            rb.velocity = new Vector2(rb.velocity.x, initialJumpSpeed);

            // ジャンプタイマーをリセット
            jumpTimer = 0f;
            isJumping = true;
        }
    }
}
