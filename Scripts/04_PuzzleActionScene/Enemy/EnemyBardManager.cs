using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 2点を行き来する敵
    /// </summary>
    public class EnemyBardManager : MonoBehaviour
    {
        /// <summary>
        /// 画像を反転させる用
        /// </summary>
        [SerializeField] private SpriteRenderer spriteRenderer;
        /// <summary>
        /// 移動を開始する位置
        /// </summary>
        [SerializeField] private Transform startTransform;
        /// <summary>
        /// 移動を切り替えしする位置
        /// </summary>
        [SerializeField] private Transform endTransform;
        /// <summary>
        /// 片道移動するのにかかる時間
        /// </summary>
        [SerializeField] private float moveTime;

        private float elapsedTime;

        private bool isMovingForward;

        private Vector3[] targetPos;
        
        /// <summary>
        /// 初期化用のメソッド
        /// </summary>
        public void Initialize()
        {
            targetPos = new[] { startTransform.position, endTransform.position };
            transform.position = startTransform.position;
            elapsedTime = 0;
            isMovingForward = true;
            spriteRenderer.flipX = true;
        }

        /// <summary>
        /// 毎フレームおこなう処理
        /// </summary>
        public void MyUpDate()
        {
            //時間を加算
            elapsedTime += Time.deltaTime;

            //前に移動させるとき
            if (isMovingForward)
            {
                //移動させる処理
                transform.position = Vector3.Lerp(targetPos[0], targetPos[1], elapsedTime / moveTime);

                //経過時間を超えたら元に戻す
                if (elapsedTime >= moveTime)
                {
                    spriteRenderer.flipX = false;
                    isMovingForward = false;
                    elapsedTime = 0f;
                }
            }
            else
            {
                transform.position = Vector3.Lerp(targetPos[1], targetPos[0], elapsedTime / moveTime);

                if (elapsedTime >= moveTime)
                {
                    spriteRenderer.flipX = true;
                    isMovingForward = true;
                    elapsedTime = 0f;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            //プレイヤーかどうかを判断
            var playerStateBehaviour = col.gameObject.GetComponent<PlayerStateBehaviour>();
            
            //プレイヤーのとき
            if (playerStateBehaviour != null)
            {
                //ICollisionEnemyを取得出来たらPlayerに通知する
                var playerState = playerStateBehaviour.PlayerStateMachine.CurrentState;
                var collisionEnemy = playerState as ICollisionEnemy;
                collisionEnemy?.CollisionEnemy();
            }
        }
    }
}
