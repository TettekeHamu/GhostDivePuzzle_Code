using TettekeKobo.StateMachine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// オハカにダイブ中 & オソナエを完了した時のState
    /// </summary>
    public class DiveOfferingState : IHamuState,ICollisionEnemy
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
        /// ダイブ終了時の機能をまとめたクラス
        /// </summary>
        private readonly PlayerDiveExitFuncManager exitFuncManager;
        
        /// <summary>
        /// コンストラクター
        /// </summary>
        public DiveOfferingState(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
            exitFuncManager = new PlayerDiveExitFuncManager(ts, pcc);
        }
        
        public void Enter()
        {
            //オソナエサキの座標に自身を合わせる
            playerComponent.transform.position = playerComponent.TombObject.TombObjectComponent.OfferingManager.NearestOfferingPlace.transform.position;
            //アニメーションの変更
            playerComponent.AnimationManager.PlayerAnimator.SetBool(playerComponent.AnimationManager.IsOffering,true);
            //オソナエできたことをオソナエサキに通知する
            playerComponent.TombObject.TombObjectComponent.OfferingManager.NearestOfferingPlace.SetOfferingObject();
        }

        public void MyUpdate()
        {
            //入力があればダイブを解除する
            if(PuzzleActionSceneInputController.Instance.StopDiveKey)
            {
                transitionState.TransitionState(PlayerStateType.Idle);
                exitFuncManager.StopDiving(
                    PlayerDiveType.DiveTomb,
                    playerComponent.TombObject.TombObjectComponent.GroundLayer,
                    playerComponent.TombObject.TombObjectComponent.ObjectLayer);
            }
        }

        public void MyFixedUpdate()
        {
            
        }

        public void Exit()
        {
            
        }
        
        public void CollisionEnemy()
        {
            transitionState.TransitionState(PlayerStateType.Dead);
        }
    }
}
