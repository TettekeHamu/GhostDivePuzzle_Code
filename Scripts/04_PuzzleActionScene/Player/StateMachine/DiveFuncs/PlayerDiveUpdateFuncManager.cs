using TettekeKobo.StateMachine;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// プレイヤーがダイブ中にUpdate() & FixedUpdate()内でおこなう処理をまとめたクラス
    /// </summary>
    public class PlayerDiveUpdateFuncManager
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
        public PlayerDiveUpdateFuncManager(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
        }
        
        /// <summary>
        /// 下に地面がないかチェックする処理
        /// </summary>
        /// <returns>下に地面があればtrueを返す</returns>
        public bool CheckOnGround()
        {
            var onGround = playerComponent.BoxCollider2D.IsTouching(playerComponent.ContactFilter2D);
            return onGround;
        }
        
        /// <summary>
        /// 近くにオソナエサキがないかチェックする処理
        /// </summary>
        /// <returns>近くにあればtrueを返す</returns>
        public bool CheckNearOfferingPlace()
        {
            var isCheck = playerComponent.TombObject.TombObjectComponent.OfferingManager.CheckNearestOfferingPosDistance();
            return isCheck;
        }

        /// <summary>
        /// プレイヤーを移動させる処理
        /// </summary>
        /// <param name="objectWight">ダイブ先のオブジェクトの重さ</param>
        public void MovePlayer(float objectWight)
        {
            var playerInputX = PuzzleActionSceneInputController.Instance.MoveAxisKey.x;
            playerComponent.Rigidbody2D.velocity = new Vector2(playerInputX * playerComponent.MoveSpeed / objectWight, 0);
        }
    }
}
