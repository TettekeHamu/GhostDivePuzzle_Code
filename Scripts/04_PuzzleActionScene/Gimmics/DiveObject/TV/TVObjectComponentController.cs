using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// TVオブジェクトのコンポーネントを管理するクラス
    /// </summary>
    public class TVObjectComponentController : MonoBehaviour
    {
        /// <summary>
        /// 周りを照らす光を管理するLight
        /// </summary>
        [SerializeField] private Light2D light2D;
        /// <summary>
        /// Rigidbody2Dコンポーネント
        /// </summary>
        [SerializeField] private Rigidbody2D rb2D;
        /// <summary>
        /// BoxCollider2Dコンポーネント
        /// </summary>
        [SerializeField] private BoxCollider2D boxCollider2D;
        /// <summary>
        /// Sprite描画用のコンポーネント
        /// </summary>
        [SerializeField] private SpriteRenderer spriteRenderer;
        /// <summary>
        /// 地面との接地フィルター
        /// </summary>
        [SerializeField] private ContactFilter2D groundContactFilter2D;
        /// <summary>
        /// オブジェクトとの接地フィルター
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
        /// <summary>
        /// 通電しているかどうか
        /// </summary>
        private bool isLighting;
        
        public BoxCollider2D BoxCollider2D => boxCollider2D;
        public Rigidbody2D Rigidbody2D => rb2D;
        public SpriteRenderer SpriteRenderer => spriteRenderer;
        public ContactFilter2D GroundContactFilter2D => groundContactFilter2D;
        public ContactFilter2D ObjectContactFilter2D => objectContactFilter2D;
        public ContactFilter2D PlayerContactFilter2D => playerContactFilter2D;
        public LayerMask GroundLayer => groundLayer;
        public LayerMask ObjectLayer => objectLayer;
        public LayerMask PlayerLayer => playerLayer;
        public bool IsLighting => isLighting;

        /// <summary>
        /// 初期化用メソッド
        /// </summary>
        public void Initialize()
        {
            isLighting = false;
        }

        /// <summary>
        /// 電気をつける・消す用のメソッド
        /// </summary>
        /// <param name="onLight"></param>
        public void TurnOnTVLight(bool onLight)
        {
            if (onLight)
            {
                light2D.intensity = 2.8f;
                isLighting = true;
            }
            else
            {
                light2D.intensity = 0f;
                isLighting = false;
            }
        }
    }
}
