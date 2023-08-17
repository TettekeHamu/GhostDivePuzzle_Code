using System;
using UniRx;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ゴールオブジェクトにアタッチするクラス
    /// </summary>
    public class GoalObjectManager : MonoBehaviour,ISwitchable
    {
        /// <summary>
        /// アニメーター
        /// </summary>
        [SerializeField] private Animator animator;
        /// <summary>
        /// アクティブかどうかを表す
        /// </summary>
        private bool isActive;
        /// <summary>
        /// ゴール処理を複数回呼ばれるのを防ぐ用のbool値
        /// </summary>
        private bool isCollisionPlayer;
        /// <summary>
        /// ゴールしたことを神クラスに伝えるSubject
        /// </summary>
        private readonly Subject<Unit> onGameClearSubject = new Subject<Unit>();

        /// <summary>
        /// アクティブかどうかを表す
        /// </summary>
        public bool IsActive => isActive;
        /// <summary>
        /// 購読用のIObservable
        /// </summary>
        public IObservable<Unit> OnGameClearObservable => onGameClearSubject;

        /// <summary>
        /// 初期化用のメソッド
        /// </summary>
        public void Initialize()
        {
            isActive = false;
            isCollisionPlayer = false;
        }
        
        public void Activate()
        {
            isActive = true;
            animator.SetBool("IsActive", true);
        }

        public void Deactivate()
        {
            
        }

        public Vector3 GetObjectPosition()
        {
            return transform.position;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var playerStateBehaviour = col.gameObject.GetComponent<PlayerStateBehaviour>();
            if (playerStateBehaviour != null && !isCollisionPlayer)
            {
                //アクティブじゃなければリターン
                if(!isActive) return;
                
                //フラグを変更
                isCollisionPlayer = true;
                //プレイヤーにゴールしたことを通知させる
                playerStateBehaviour.StopMoving();
                //マネージャーにゴールしたことを伝える
                onGameClearSubject.OnNext(Unit.Default);
            }
        }
    }
}
