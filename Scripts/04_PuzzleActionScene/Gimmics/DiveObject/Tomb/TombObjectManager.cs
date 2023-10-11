using System;
using UniRx;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// お墓のオブジェクトにアタッチするクラス
    /// </summary>
    public class TombObjectManager : MonoBehaviour
    {
        /// <summary>
        /// 現在のState(debug用)
        /// </summary>
        [SerializeField] private TombStateType currentTombState;
        /// <summary>
        /// オハカのコンポーネントを管理するクラス
        /// </summary>
        [SerializeField] private TombObjectComponentController tombObjectComponent;
        /// <summary>
        /// お墓の状態を管理するStateMachine
        /// </summary>
        private TombObjectStateMachine tombObjectStateMachine;

        public TombObjectComponentController TombObjectComponent => tombObjectComponent;

        /*
        private void OnCollisionStay2D(Collision2D collision)
        {
            //プレイヤーかどうかを判断
            var playerStateBehaviour = collision.gameObject.GetComponent<PlayerStateBehaviour>();
            
            //プレイヤーのとき
            if (playerStateBehaviour != null)
            {
                //CollisionPlayer(playerStateBehaviour);
            }
        }
        */

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
                //オソナエ済みの状態ならここでとめる
                if(tombObjectComponent.OfferingManager.IsCompletedOffering) return;
                    
                //憑依を始める処理
                //プレイヤーに自身を設定する
                player.SetTombObject(this);
                //プレイヤーのゴーストダイブを開始させる
                var startDive = player.PlayerStateMachine.CurrentState as IStartDiveable;
                startDive?.StartTombDive();
            }
        }
        
        /// <summary>
        /// ダイブが解除されたときにおこなうメソッド
        /// </summary>
        public void StopDiving()
        {
            tombObjectStateMachine.StopDiving();
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
            tombObjectStateMachine.TransitionDiveState();
        }

        /// <summary>
        /// 初期化用のメソッド
        /// </summary>
        public void Initialize()
        {
            //Rigidbody周りの設定
            tombObjectComponent.Rigidbody2D.gravityScale = 1;
            //stateを設定
            tombObjectStateMachine = new TombObjectStateMachine(tombObjectComponent);
            tombObjectStateMachine.OnChangeStateObservable
                .Subscribe(x => currentTombState = x)
                .AddTo(this);
            tombObjectStateMachine.Initialize(TombStateType.NonDivedIdling);
        }

        public void MyUpdate()
        {
            tombObjectStateMachine.MyUpdate();
        }

        public void MyFixedUpdate()
        {
            tombObjectStateMachine.MyFixedUpdate();
        }
    }
}
