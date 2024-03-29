using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle.Refactoring
{
    /// <summary>
    /// DiveObjectのコンポーネントを管理する親クラス
    /// </summary>
    public class DiveObjectComponentController : MonoBehaviour
    {
        /// <summary>
        /// Colliderコンポーネント
        /// </summary>
        [SerializeField] private BoxCollider2D boxCollider2D;
        /// <summary>
        /// Colliderコンポーネント
        /// </summary>
        [SerializeField] private CircleCollider2D circleCollider2D;
        /// <summary>
        /// Rigidbody2dコンポーネント
        /// </summary>
        [SerializeField] private Rigidbody2D rb2D;
        /// <summary>
        /// Sprite描画用のコンポーネント
        /// </summary>
        [SerializeField] private SpriteRenderer spriteRenderer;
        /// <summary>
        /// 地面との接地フィルター(着地判定に使用)
        /// </summary>
        [SerializeField] private ContactFilter2D groundContactFilter2D;
        /// <summary>
        /// オブジェクトとの接地フィルター(着地判定に使用)
        /// </summary>
        [SerializeField] private ContactFilter2D objectContactFilter2D;
        /// <summary>
        /// プレイヤーとの接地フィルター
        /// </summary>
        [SerializeField] private ContactFilter2D playerContactFilter2D;
        /// <summary>
        /// 地面との判定用のLayer
        /// </summary>
        [SerializeField] private LayerMask groundLayer;
        /// <summary>
        /// オブジェクトとの判定用のLayer
        /// </summary>
        [SerializeField] private LayerMask objectLayer;
        /// <summary>
        /// プレイヤーとの判定用のLayer
        /// </summary>
        [SerializeField] private LayerMask playerLayer;
        
        public BoxCollider2D BoxCollider2D => boxCollider2D;
        public CircleCollider2D CircleCollider2D => circleCollider2D;
        public Rigidbody2D Rigidbody2D => rb2D;
        public SpriteRenderer SpriteRenderer => spriteRenderer;
        public ContactFilter2D GroundContactFilter2D => groundContactFilter2D;
        public ContactFilter2D ObjectContactFilter2D => objectContactFilter2D;
        public ContactFilter2D PlayerContactFilter2D => playerContactFilter2D;
        public LayerMask GroundLayer => groundLayer;
        public LayerMask ObjectLayer => objectLayer;
        public LayerMask PlayerLayer => playerLayer;
    }
}
