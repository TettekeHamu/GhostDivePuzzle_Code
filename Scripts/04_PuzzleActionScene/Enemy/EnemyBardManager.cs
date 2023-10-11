using System;
using naichilab.EasySoundPlayer.Scripts;
using UniRx;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 2点を行き来する敵
    /// </summary>
    public class EnemyBardManager : MonoBehaviour
    {
        /// <summary>
        /// 死んだときに再生するときに
        /// </summary>
        [SerializeField] private ParticleSystem deadParticle;
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

        private readonly Subject<Unit> onDestroySubject = new Subject<Unit>();

        public IObservable<Unit> OnDestroyObservable => onDestroySubject;

        /// <summary>
        /// 初期化用のメソッド
        /// </summary>
        public void Initialize()
        {
            targetPos = new[] { startTransform.position, endTransform.position };
            transform.position = startTransform.position;
            elapsedTime = 0;
            isMovingForward = true;
            if (startTransform.position.x > endTransform.position.x)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }

        /// <summary>
        /// 毎フレームおこなう処理
        /// </summary>
        public void MyUpDate()
        {
            //if(startTransform.position.x == endTransform.position.x) return;

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
                    spriteRenderer.flipX = !spriteRenderer.flipX;
                    isMovingForward = false;
                    elapsedTime = 0f;
                }
            }
            else
            {
                transform.position = Vector3.Lerp(targetPos[1], targetPos[0], elapsedTime / moveTime);

                if (elapsedTime >= moveTime)
                {
                    spriteRenderer.flipX = !spriteRenderer.flipX;
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
                //プレイヤーが敵を倒せる状態かどうか調べる
                if (playerStateBehaviour.ComponentController.CanDestroyEnemy)
                {
                    Instantiate(deadParticle, transform.position, Quaternion.identity);
                    onDestroySubject.OnNext(Unit.Default);
                    playerStateBehaviour.ComponentController.DestroyEnemy();
                    SePlayer.Instance.Play("SE_EnemyDestroy");
                    gameObject.SetActive(false); //Destroyだとエラーが出るので非表示に変更させる
                    return;
                }
                
                //ICollisionEnemyを取得出来たらPlayerに通知する
                var playerState = playerStateBehaviour.PlayerStateMachine.CurrentState;
                var collisionEnemy = playerState as ICollisionEnemy;
                collisionEnemy?.CollisionEnemy();
            }
        }
    }
}
