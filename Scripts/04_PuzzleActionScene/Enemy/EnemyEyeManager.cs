using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// プレイヤーを追従する敵
    /// </summary>
    public class EnemyEyeManager : MonoBehaviour
    {
        /// <summary>
        /// アニメーター
        /// </summary>
        [SerializeField] private Animator animator;
        /// <summary>
        /// 移動の開始地点
        /// </summary>
        [SerializeField] private Transform startTransform;
        /// <summary>
        /// 移動の切り替えし地点
        /// </summary>
        [SerializeField] private Transform endTransform;
        /// <summary>
        /// 移動させる時間
        /// </summary>
        [SerializeField] private float moveTime;
        /// <summary>
        /// プレイヤーを感知する円の半径
        /// </summary>
        [SerializeField] private int searchingRange;
        /// <summary>
        /// 追うときのスピード
        /// </summary>
        [SerializeField] private int chaseSpeed;

        /// <summary>
        /// アニメーション名
        /// </summary>
        private int animationName = Animator.StringToHash("IsChasing");
        /// <summary>
        /// プレイヤー
        /// </summary>
        private PlayerStateBehaviour player;
        /// <summary>
        /// 移動の折り返し地点
        /// </summary>
        private Vector3[] targetPos;
        /// <summary>
        /// 移動開始地点
        /// </summary>
        private Vector3 startPoint;
        /// <summary>
        /// 巡回中かどうか
        /// </summary>
        private bool isLooping;
        /// <summary>
        /// 経過時間
        /// </summary>
        private float elapsedTime;
        /// <summary>
        /// 移動方向
        /// </summary>
        private bool isMovingForward;

        public void Initialize()
        {
            //プレイヤーを取得
            player = FindObjectOfType<PlayerStateBehaviour>();
            //targetPosに開始地点と折り返し地点を格納
            var position = startTransform.position;
            targetPos = new[] { position, endTransform.position};
            //開始点に移動
            transform.position = position;
            startPoint = position;
            //経過時間を0に
            elapsedTime = 0;
            //向きは真っ直ぐ
            isMovingForward = true;
            //デフォルトはループさせる
            isLooping = true;
        }
        
        public void MyUpDate()
        {
            //経過時間を加算
            elapsedTime += Time.deltaTime;
            //プレイヤーの位置を格納
            var playerPos = player.transform.position;
            //プレイヤーとの距離を算出
            var position = transform.position;
            var distance = (playerPos.x - position.x) * (playerPos.x - position.x) 
                             + (playerPos.y - position.y) * (playerPos.y - position.y);
            
            if (distance <= searchingRange * searchingRange)
            {
                animator.SetBool(animationName, true);
                isLooping = false;
                transform.position = Vector2.MoveTowards(transform.position, playerPos, chaseSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool(animationName, false);
                if(!isLooping)
                {
                    //元の位置に戻す
                    transform.position = Vector2.MoveTowards(transform.position, startPoint, 3 * Time.deltaTime);
                    if(transform.position == startPoint)
                    {
                        isLooping = true;
                        isMovingForward = true;
                        elapsedTime = 0f;
                    }
                }
                else
                {
                    //二点を行き来させる
                    if (isMovingForward)
                    {
                        transform.position = Vector3.Lerp(targetPos[0], targetPos[1], elapsedTime / moveTime);
                          
                        if (elapsedTime >= moveTime)
                        {
                            isMovingForward = false;
                            elapsedTime = 0f;
                        }
                    }
                    else
                    {
                        transform.position = Vector3.Lerp(targetPos[1], targetPos[0], elapsedTime / moveTime);

                        if (elapsedTime >= moveTime)
                        {
                            isMovingForward = true;
                            elapsedTime = 0f;
                        }
                    }
                }
            }
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            var playerStateBehaviour = col.gameObject.GetComponent<PlayerStateBehaviour>();
            
            if (playerStateBehaviour != null)
            {
                var playerState = playerStateBehaviour.PlayerStateMachine.CurrentState;
                var collisionEnemy = playerState as ICollisionEnemy;
                collisionEnemy?.CollisionEnemy();
            }
        }
    }
}
