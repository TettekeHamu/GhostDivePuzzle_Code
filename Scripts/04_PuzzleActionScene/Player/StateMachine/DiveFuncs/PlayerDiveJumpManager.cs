using TettekeKobo.StateMachine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ダイブ時のジャンプ処理をまとめたクラス
    /// </summary>
    public class PlayerDiveJumpManager
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
        public PlayerDiveJumpManager(ITransitionState<PlayerStateType> ts, PlayerComponentController pcc)
        {
            transitionState = ts;
            playerComponent = pcc;
        }
    }
}
