using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// テレビのオブジェクトを管理するクラス
    /// </summary>
    public class TVObjectManager : MonoBehaviour
    {
        /// <summary>
        /// TVオブジェクトのコンポーネントを管理するクラス
        /// </summary>
        [SerializeField] private TVObjectComponentController tvObjectComponentController;
        /// <summary>
        /// TVオブジェクトのStateを管理するクラス
        /// </summary>
        private TVStateMachine tvStateMachine;

        /// <summary>
        /// 外部（主にプレイヤー）からTVにアタッチされているコンポーネントを取得するようのgetter
        /// </summary>
        public TVObjectComponentController TVObjectComponentController => tvObjectComponentController;

        /// <summary>
        /// 初期化用のメソッド
        /// </summary>
        public void Initialize()
        {
            //Rigidbody周りの設定
            tvObjectComponentController.Rigidbody2D.gravityScale = 1;
            //stateを設定
            tvStateMachine = new TVStateMachine(tvObjectComponentController);
            tvStateMachine.Initialize(TVStateType.NonDivedIdling);
            tvObjectComponentController.Initialize();
        }

        /// <summary>
        /// ダイブが解除されたときにおこなうメソッド
        /// </summary>
        public void StopDiving()
        {
            tvStateMachine.StopDiving();
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
            tvStateMachine.TransitionDiveState();
        }

        public void MyUpdate()
        {
            tvStateMachine.MyUpdate();
        }

        public void MyFixedUpdate()
        {
            tvStateMachine.MyFixedUpdate();
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
                    playerStateBehaviour.SetTVObject(this);
                    //プレイヤーのゴーストダイブを開始させる
                    var startDive = playerStateBehaviour.PlayerStateMachine.CurrentState as IStartDiveable;
                    startDive?.StartTVDive();
                }
            }
        }
    }
}
