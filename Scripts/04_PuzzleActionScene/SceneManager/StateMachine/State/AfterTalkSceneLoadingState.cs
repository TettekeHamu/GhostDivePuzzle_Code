using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// シーンクリア後の会話シーンに移行させるState
    /// </summary>
    public class AfterTalkSceneLoadingState : IHamuState
    {
        public void Enter()
        {
            SceneLoadManager.Instance.LoadNextScene("AfterTalkScene");
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
