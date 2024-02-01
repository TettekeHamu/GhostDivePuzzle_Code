using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle.Refactoring
{
    /// <summary>
    /// 電子レンジのGameObjectを管理するクラス
    /// </summary>
    public class MicrowaveObjectBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 攻撃できるオブジェクトのコンポーネントを管理するクラス
        /// </summary>
        [SerializeField] private AttackableObjectComponentController componentController;
        /// <summary>
        /// お墓の状態を管理するStateMachine
        /// </summary>
        private DiveObjectStateMachine stateMachine;
        /// <summary>
        /// 外部からコンポーネントをいじる用のgetter
        /// </summary>
        public AttackableObjectComponentController ComponentController => componentController;

        private void OnTriggerStay2D(Collider2D other)
        {
            //プレイヤーかどうかを判断
            var playerStateBehaviour = other.gameObject.GetComponent<PlayerStateBehaviour>();
            
            //プレイヤーのとき
            if (playerStateBehaviour != null)
            {
                CollisionPlayer(playerStateBehaviour);
            }
        }

        private void CollisionPlayer(PlayerStateBehaviour player)
        {
            //プレイヤー側がダイブ用の入力を入れているか確認
            //IHamuStateを取得
            var playerState = player.PlayerStateMachine.CurrentState;
            //IHamuStateをIDiveableに変換
            var canDiveTombObject = playerState as IDiveable;
            //変換出来たらGetCanDive()を実行
            var canDive = canDiveTombObject?.GetCanDive(); 

            //入力がある場合
            if (canDive == true)
            {
                //憑依を始める処理
                //プレイヤーに自身を設定する
                player.SetMicrowaveObject(this);
                //プレイヤーのゴーストダイブを開始させる
                var startDive = player.PlayerStateMachine.CurrentState as IStartDiveable;
                startDive?.StartMicrowaveDive();
            }
        }
        
        /// <summary>
        /// ダイブが解除されたときにおこなうメソッド
        /// </summary>
        public void StopDiving()
        {
            stateMachine.StopDiving();
        }
        
        /// <summary>
        /// Playerの位置をお墓に合わせるメソッド
        /// </summary>
        /// <param name="playerTransform">PlayerのTransform</param>
        public void SetPlayerTransform(Transform playerTransform)
        {
            //プレイヤーを自身の位置までもっていき、プレイヤーを親オブジェクトに設定
            playerTransform.position = transform.position;
            transform.parent = playerTransform;
            transform.localPosition = Vector3.zero;
            //Stateを変更(ここで変更しないと墓がすり抜ける)
            stateMachine.TransitionDiveState();
        }

        /// <summary>
        /// 初期化用のメソッド
        /// </summary>
        public void Awake()
        {
            //Rigidbody周りの設定
            componentController.Rigidbody2D.gravityScale = 1;
            //stateを設定
            stateMachine = new AttackableObjectStateMachine(componentController);
        }

        public void Update()
        {
            stateMachine.MyUpdate();
        }

        public void FixedUpdate()
        {
            stateMachine.MyFixedUpdate();
        }
    }
}
