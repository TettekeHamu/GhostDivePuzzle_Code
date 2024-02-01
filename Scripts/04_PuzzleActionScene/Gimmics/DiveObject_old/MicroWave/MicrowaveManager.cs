using UniRx;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 
    /// </summary>
    public class MicrowaveManager : MonoBehaviour
    {
        /// <summary>
        /// 現在のState(debug用)
        /// </summary>
        [SerializeField] private MicrowaveStateType currentState;
        /// <summary>
        /// オハカのコンポーネントを管理するクラス
        /// </summary>
        [SerializeField] private MicrowaveComponentController componentController;
        /// <summary>
        /// お墓の状態を管理するStateMachine
        /// </summary>
        private MicrowaveStateMachine stateMachine;
        /// <summary>
        /// 
        /// </summary>
        public MicrowaveComponentController ComponentController => componentController;

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
        public void Initialize()
        {
            //Rigidbody周りの設定
            componentController.Rigidbody2D.gravityScale = 1;
            //stateを設定
            stateMachine = new MicrowaveStateMachine(componentController);
            stateMachine.OnChangeStateObservable
                .Subscribe(x => currentState = x)
                .AddTo(this);
            stateMachine.Initialize(MicrowaveStateType.NonDivedIdling);
        }

        public void MyUpdate()
        {
            stateMachine.MyUpdate();
        }

        public void MyFixedUpdate()
        {
            stateMachine.MyFixedUpdate();
        }
    }
}
