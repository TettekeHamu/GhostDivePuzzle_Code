namespace TettekeKobo.StateMachine
{
    /// <summary>
    /// Stateに関する処理(メソッド)をもつInterface
    /// </summary>
    public interface IHamuState
    {
        /// <summary>
        /// Stateに入った時に1度だけ実行する処理
        /// </summary>
        public void Enter();
        /// <summary>
        /// 毎フレーム実行するクラス処理
        /// </summary>
        public void MyUpdate();
        /// <summary>
        /// 等間隔で実行する処理
        /// </summary>
        public void MyFixedUpdate();
        /// <summary>
        /// Stateから出るときに1度だけ実行する処理
        /// </summary>
        public void Exit();
    }
}
