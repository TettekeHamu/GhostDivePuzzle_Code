using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 扇風機オブジェクトにアタッチするクラス
    /// </summary>
    public class FanObjectManager : MonoBehaviour
    {
        /// <summary>
        /// 扇風機のコンポーネントを管理するクラス
        /// </summary>
        [SerializeField] private FanObjectComponentController fanObjectComponentController;
        /// <summary>
        /// 扇風機のStateを管理するクラス
        /// </summary>
        private FanStateMachine fanStateMachine;

        /// <summary>
        /// 外部（主にプレイヤー）から扇風機にアタッチされているコンポーネントを取得するようのgetter
        /// </summary>
        public FanObjectComponentController FanObjectComponentController => fanObjectComponentController;
        

        /// <summary>
        /// 初期化用のメソッド
        /// </summary>
        public void Initialize()
        {
            //Rigidbody周りの設定
            fanObjectComponentController.Rigidbody2D.gravityScale = 1;
            //stateを設定
            fanStateMachine = new FanStateMachine(fanObjectComponentController);
            fanStateMachine.Initialize(FanStateType.NonDivedIdling);
        }

        /// <summary>
        /// ダイブが解除されたときにおこなうメソッド
        /// </summary>
        public void StopDiving()
        {
            fanStateMachine.StopDiving();
        }
        
        /// <summary>
        /// Playerの位置をお墓に合わせるメソッド
        /// </summary>
        /// <param name="playerTransform">PlayerのTransform</param>
        public void SetPlayerTransform(Transform playerTransform)
        {
            //プレイヤーを自身の位置までもっていき、プレイヤーを親オブジェクトに設定
            var tr = transform;
            playerTransform.position = tr.position;
            tr.parent = playerTransform;
            tr.localPosition = Vector3.zero;
            //Stateを変更(ここで変更しないと墓がすり抜ける)
            fanStateMachine.TransitionDiveState();
        }

        public void MyUpdate()
        {
            fanStateMachine.MyUpdate();
        }

        public void MyFixedUpdate()
        {
            fanStateMachine.MyFixedUpdate();
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            //プレイヤーかどうかを判断
            var playerStateBehaviour = collision.gameObject.GetComponent<PlayerStateBehaviour>();
            
            //プレイヤーのとき
            if (playerStateBehaviour != null)
            {
                //プレイヤー側がダイブ用の入力を入れているか確認
                //IHamuStateを取得
                var playerState = playerStateBehaviour.PlayerStateMachine.CurrentState;
                //IHamuStateをIDiveableに変換
                var canDiveTombObject = playerState as IDiveable;
                //変換出来たらGetCanDive()を実行
                var canDive = canDiveTombObject?.GetCanDive(); 

                //入力がある場合
                if (canDive == true)
                {
                    //憑依を始める処理
                    //プレイヤーに自身を設定する
                    playerStateBehaviour.SetFanObject(this);
                    //プレイヤーのゴーストダイブを開始させる
                    var startDive = playerStateBehaviour.PlayerStateMachine.CurrentState as IStartDiveable;
                    startDive?.StartFanDive();
                }
            }
        }
    }
}
