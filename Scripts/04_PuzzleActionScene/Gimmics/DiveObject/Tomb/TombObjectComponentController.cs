using System;
using UnityEngine;
using UnityEngine.Animations;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// オハカのコンポーネントを管理するクラス
    /// </summary>
    public class TombObjectComponentController : MonoBehaviour
    {
        /// <summary>
        /// オソナエに関する機能を持つクラス
        /// </summary>
        [SerializeField] private OfferingManager offeringManager;
        /// <summary>
        /// Colliderコンポーネント
        /// </summary>
        [SerializeField] private BoxCollider2D boxCollider2D;
        /// <summary>
        /// Rigidbody2dコンポーネント
        /// </summary>
        [SerializeField] private Rigidbody2D rb2D;
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

        public OfferingManager OfferingManager => offeringManager;
        public BoxCollider2D BoxCollider2D => boxCollider2D;
        public Rigidbody2D Rigidbody2D => rb2D;
        public ContactFilter2D GroundContactFilter2D => groundContactFilter2D;
        public ContactFilter2D ObjectContactFilter2D => objectContactFilter2D;
        public ContactFilter2D PlayerContactFilter2D => playerContactFilter2D;
        public LayerMask GroundLayer => groundLayer;
        public LayerMask ObjectLayer => objectLayer;
        public LayerMask PlayerLayer => playerLayer;
    }
}
