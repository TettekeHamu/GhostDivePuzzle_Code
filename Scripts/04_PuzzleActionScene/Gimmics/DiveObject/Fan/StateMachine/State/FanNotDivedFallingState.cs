using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// 
    /// </summary>
    public class FanNotDivedFallingState : IHamuState
    {
        /// <summary>
        /// Stateを変更する処理を持つインタフェース
        /// </summary>
        private readonly ITransitionState<FanStateType> transitionState;
        /// <summary>
        /// 扇風機のコンポーネントをまとめたクラス
        /// </summary>
        private readonly FanObjectComponentController fanObjectComponent;
        
        /// <summary>
        /// コンストラクター
        /// </summary>
        public FanNotDivedFallingState(ITransitionState<FanStateType> ts, FanObjectComponentController focc)
        {
            transitionState = ts;
            fanObjectComponent = focc;
        }
        
        public void Enter()
        {
            //下方向のみ物理演算で動けるようにする
            fanObjectComponent.Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            var constraints2D = fanObjectComponent.Rigidbody2D.constraints;
            constraints2D |= RigidbodyConstraints2D.FreezePositionX;
            constraints2D |= RigidbodyConstraints2D.FreezeRotation;
            constraints2D &= ~RigidbodyConstraints2D.FreezePositionY;
            fanObjectComponent.Rigidbody2D.constraints = constraints2D;
        }

        public void MyUpdate()
        {
            //下にプレイヤーにいるかチェックする
            var onPlayer = fanObjectComponent.Rigidbody2D.IsTouching(fanObjectComponent.PlayerContactFilter2D);
            if (onPlayer)
            {
                var playerStateBehaviour = Object.FindObjectOfType<PlayerStateBehaviour>();
                //ダイブしていなかったらリターン
                if(!playerStateBehaviour.PlayerStateMachine.IsDiving) return;
                transitionState.TransitionState(FanStateType.NonDivedOnPlayerStaying);
                return;
            }

            //下にオブジェクトや地面がないかチェックする
            var onGround = fanObjectComponent.Rigidbody2D.IsTouching(fanObjectComponent.GroundContactFilter2D);
            var onObject = fanObjectComponent.Rigidbody2D.IsTouching(fanObjectComponent.ObjectContactFilter2D);
            if(onGround || onObject) transitionState.TransitionState(FanStateType.NonDivedIdling);
        }

        public void MyFixedUpdate()
        {
            //下に落下させる
            fanObjectComponent.Rigidbody2D.velocity = Vector2.down * 5f;
        }

        public void Exit()
        {
            fanObjectComponent.Rigidbody2D.velocity = Vector2.zero;
        }
    }
}
