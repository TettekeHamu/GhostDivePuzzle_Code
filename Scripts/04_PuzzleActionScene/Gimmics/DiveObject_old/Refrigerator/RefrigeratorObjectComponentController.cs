using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 冷蔵庫のコンポーネントを管理するクラス
    /// </summary>
    public class RefrigeratorObjectComponentController : MonoBehaviour
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
        /// 電源がついてる時に使用する用のParticle
        /// </summary>
        [SerializeField] private ParticleSystem lightningParticle;
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
        /// <summary>
        /// 電源をつけているかどうか
        /// </summary>
        private bool isRightOn;
        /// <summary>
        /// 電力を使い果たしたかどうか
        /// </summary>
        private bool isRunningOutPower;
        
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
        public bool IsRunningOutPower => isRunningOutPower;

        private void Awake()
        {
            isRightOn = false;
            lightningParticle.gameObject.SetActive(false);
            isRunningOutPower = true;
        }

        /// <summary>
        /// 電源を切り替える処理
        /// </summary>
        /// <returns></returns>
        public bool ChangeRight(bool isInPlayer = true)
        {
            if (isInPlayer)
            {
                if(!isRunningOutPower) return false;
                isRightOn = !isRightOn;
                lightningParticle.gameObject.SetActive(isRightOn);                
            }
            else
            {
                isRightOn = false;
                lightningParticle.gameObject.SetActive(false);   
            }

            return isRightOn;
        }

        /// <summary>
        /// 電源を切る処理
        /// </summary>
        public void RunOutPower()
        {
            isRunningOutPower = false;
            lightningParticle.gameObject.SetActive(false);
        } 
    }
}
