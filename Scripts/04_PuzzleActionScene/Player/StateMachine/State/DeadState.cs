using TettekeKobo.StateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 死んでいるときのState
    /// </summary>
    public class DeadState : IHamuState
    {
        /// <summary>
        /// Stateを変更させる処理を持つインタフェース
        /// </summary>
        private readonly ITransitionState<PlayerStateType> transitionState;
        /// <summary>
        /// プレイヤーのコンポーネントをまとめたクラス
        /// </summary>
        private readonly PlayerComponentController playerComponent;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public DeadState(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
        }
        
        public void Enter()
        {
            Debug.Log("やり直し！！");
            playerComponent.Rigidbody2D.velocity = Vector2.zero;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //SceneLoadManager.Instance.LoadNextScene(SceneManager.GetActiveScene().name);
        }

        public void MyUpdate()
        {
            
        }

        public void MyFixedUpdate()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}
