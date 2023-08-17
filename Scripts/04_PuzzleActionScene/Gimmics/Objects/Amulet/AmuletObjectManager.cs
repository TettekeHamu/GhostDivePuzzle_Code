using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// オフダにアタッチするクラス
    /// </summary>
    public class AmuletObjectManager : MonoBehaviour
    {
        /// <summary>
        /// 当たり判定用のコンポーネント
        /// </summary>
        [SerializeField] private BoxCollider2D boxCollider2D;

        private void OnTriggerEnter2D(Collider2D col)
        {
            var playerStateBehaviour = col.GetComponent<PlayerStateBehaviour>();
            if (playerStateBehaviour != null)
            {
                var isDiving = playerStateBehaviour.PlayerStateMachine.IsDiving;
                if (!isDiving)
                {
                    //ダイブしていなかったら通れなくする
                    boxCollider2D.isTrigger = false;
                }
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            //元に戻す
            boxCollider2D.isTrigger = true;
        }
    }
}
