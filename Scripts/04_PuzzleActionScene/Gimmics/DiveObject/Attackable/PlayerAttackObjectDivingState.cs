using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle.Refactoring
{
    /// <summary>
    /// アタックできるオブジェクトがプレイヤーにダイブされているときのState
    /// </summary>
    public class PlayerAttackObjectDivingState : PlayerDivingState
    {
        /// <summary>
        /// 攻撃できるダイブオブジェクトのコンポーネントを管理する子クラス
        /// </summary>
        private readonly AttackableObjectComponentController attackableComponentController;
        
        /// <summary>
        /// コンストラクター、第二引数を変更している！！
        /// </summary>
        public PlayerAttackObjectDivingState(AttackableObjectComponentController docc) : base(docc)
        {
            attackableComponentController = docc;
        }

        protected override void OnEnter()
        {
            Debug.Log("アタックオブジェクトがプレイヤーにダイブされました");
        }

        protected override void OnExit()
        {
            //電源を切る
            attackableComponentController.ChangeRight(false);
        }
    }
}