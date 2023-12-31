using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// プレイヤーのコンポーネントをまとめたクラス
    /// </summary>
    public class PlayerComponentController : MonoBehaviour
    {
        /// <summary>
        /// 当たり判定用のコンポーネント
        /// </summary>
        [SerializeField] private BoxCollider2D boxCollider2D;
        /// <summary>
        /// プレイヤーを動かす用のコンポーネントを格納
        /// </summary>
        [SerializeField] private Rigidbody2D rb2D;
        /// <summary>
        /// プレイヤーの描画をおこなうコンポーネント
        /// </summary>
        [SerializeField] private SpriteRenderer spriteRenderer;
        /// <summary>
        /// アニメーションを管理するコンポーネント
        /// </summary>
        [SerializeField] private PlayerAnimationManager animationManager;
        /// <summary>
        /// パーティクルを管理するコンポーネント
        /// </summary>
        [SerializeField] private PlayerParticleManager particleManager;
        /// <summary>
        /// プレイヤーに追従するライト
        /// </summary>
        [SerializeField] private PlayerLightManager[] playerLights;
        /// <summary>
        /// お墓の親オブジェクトのTransform
        /// </summary>
        [SerializeField] private Transform diveObjectTilemapTransform;
        /// <summary>
        /// 接地判定用のコンポーネント
        /// </summary>
        [SerializeField] private ContactFilter2D contactFilter2D;
        /// <summary>
        /// プレイヤーの最大ライフ
        /// </summary>
        [SerializeField] private float maxPlayerLife;
        /// <summary>
        /// 移動スピード
        /// </summary>
        [SerializeField] private float moveSpeed;
        /// <summary>
        /// 落下スピード
        /// </summary>
        [SerializeField] private float fallSpeed;
        /// <summary>
        /// プレイヤーのUIを管理するクラス
        /// </summary>
        private PlayerUIManager playerUIManager;
        /// <summary>
        /// プレイヤーの現在のライフ
        /// </summary>
        private float currentPlayerLife;
        /// <summary>
        /// 敵を倒せるかどうか（trueなら倒せる）
        /// </summary>
        private bool canDestroyEnemy;
        /// <summary>
        /// ダイブ先のオハカ
        /// </summary>
        private TombObjectManager tombObject;
        /// <summary>
        /// ダイブ先のTV
        /// </summary>
        private TVObjectManager tvObject;
        /// <summary>
        /// ダイブ先の扇風機
        /// </summary>
        private FanObjectManager fanObject;
        /// <summary>
        /// ダイブ先の冷蔵庫
        /// </summary>
        private RefrigeratorObjectManager refrigeratorObject;
        /// <summary>
        /// ダイブ先の電子レンジ
        /// </summary>
        private MicrowaveManager microwaveObject;

        public BoxCollider2D BoxCollider2D => boxCollider2D;
        public Rigidbody2D Rigidbody2D => rb2D;
        public SpriteRenderer SpriteRenderer => spriteRenderer;
        public PlayerAnimationManager AnimationManager => animationManager;
        public PlayerParticleManager ParticleManager => particleManager;
        public PlayerLightManager[] PlayerLights => playerLights;
        public Transform DiveObjectTilemapTransform => diveObjectTilemapTransform;
        public ContactFilter2D ContactFilter2D => contactFilter2D;
        public float MoveSpeed => moveSpeed;
        public float FallSpeed => fallSpeed;
        public bool CanDestroyEnemy => canDestroyEnemy;
        public TombObjectManager TombObject => tombObject;
        public TVObjectManager TVObject => tvObject;
        public FanObjectManager FanObject => fanObject;
        public RefrigeratorObjectManager RefrigeratorObject => refrigeratorObject;
        public MicrowaveManager MicrowaveObject => microwaveObject;

        public void Initialize(PlayerUIManager uiManager)
        {
            //敵を倒せなくさせる
            canDestroyEnemy = false;
            //UIManagerをSceneManagerから受け取る
            playerUIManager = uiManager;
            //ライフを設定
            currentPlayerLife = maxPlayerLife;
            //パーティクルを再生
            particleManager.PlayTrailParticle();
            //パーティクルを追従させる
            this.UpdateAsObservable()
                .Subscribe(_ => particleManager.FollowPlayer(transform.position))
                .AddTo(this);
        }
        
        //敵を倒せるようにする処理
        public void ChangeCanDestroyEnemy(bool can)
        {
            canDestroyEnemy = can;
        }

        /// <summary>
        /// 敵を倒すときに呼び出される処理
        /// </summary>
        public void DestroyEnemy()
        {
            if (microwaveObject != null)
            {
                microwaveObject.ComponentController.RunOutPower();
                canDestroyEnemy = false;
            }
            else if (refrigeratorObject != null)
            {
                refrigeratorObject.ComponentController.RunOutPower();
                canDestroyEnemy = false;
            }
        }

        /// <summary>
        /// Lifeを減らす処理
        /// </summary>
        /// <param name="amount">減らす量</param>
        /// <returns>0以下になったらtrueを返す</returns>
        public bool ReduceLife(float amount)
        {
            currentPlayerLife -= amount;
            playerUIManager.ReduceLifeGauge(currentPlayerLife, maxPlayerLife);
            if (currentPlayerLife <= 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Lifeをマックスに戻す処理
        /// </summary>
        public void RecoverLife()
        {
            playerUIManager.RecoveryLifeGauge(currentPlayerLife,maxPlayerLife);
            currentPlayerLife = maxPlayerLife;
        }

        /// <summary>
        /// ダイブ先のオハカを設定する処理、Playerが呼びだす
        /// </summary>
        public void SetTombObject(TombObjectManager tomb)
        {
            tombObject = tomb;
        }

        /// <summary>
        /// ダイブ先のTVを設定する処理、Playerが呼びだす
        /// </summary>
        public void SetTVObject(TVObjectManager tv)
        {
            tvObject = tv;
        }

        /// <summary>
        /// ダイブ先の扇風機を設定する処理、Playerが呼びだす
        /// </summary>
        public void SetFanObject(FanObjectManager fan)
        {
            fanObject = fan;
        }
        
        /// <summary>
        /// ダイブ先の冷蔵庫を設定する処理
        /// </summary>
        public void SetRefrigeratorObject(RefrigeratorObjectManager refrigerator)
        {
            refrigeratorObject = refrigerator;
        }

        /// <summary>
        /// ダイブ先の電子レンジを設定する処理
        /// </summary>
        public void SetMicrowaveObject(MicrowaveManager microwave)
        {
            microwaveObject = microwave;
        }
        
        /// <summary>
        /// オハカへのダイブをやめる際の処理、Playerが呼びだす
        /// </summary>
        public void StopDiving()
        {
            tombObject.TombObjectComponent.SpriteRenderer.gameObject.SetActive(true);
            tombObject.StopDiving();
            tombObject = null;
        }

        /// <summary>
        /// TVへのダイブをやめる際の処理、Playerが呼びだす
        /// </summary>
        public void StopTVDiving()
        {
            tvObject.StopDiving();
            tvObject = null;
        }

        /// <summary>
        /// Fanへのダイブをやめる際の処理、Playerが呼びだす
        /// </summary>
        public void StopFanDiving()
        {
            fanObject.StopDiving();
            fanObject = null;
        }

        /// <summary>
        /// 冷蔵庫へのダイブをやめる際の処理、Playerが呼びだす
        /// </summary>
        public void StopRefrigeratorDiving()
        {
            refrigeratorObject.StopDiving();
            refrigeratorObject = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopMicrowaveDiving()
        {
            microwaveObject.StopDiving();
            microwaveObject = null;
        }
    }
}
