using System;

namespace TettekeKobo.StateMachine
{
    /// <summary>
    /// Stateを管理する処理をもつInterface(StateMachineクラスに実装)
    /// </summary>
    /// <typeparam name="T">変更したいIStateに対応するEnum</typeparam>
    public interface IHamuStateMachine<in T> where T : Enum
    {
        /// <summary>
        /// EnumをIStateに変更する処理
        /// </summary>
        /// <param name="stateType">Enumを受け取る</param>
        /// <returns>Stateを返す</returns>
        public IHamuState GetStateFromEnum(T stateType);
        
        /// <summary>
        /// 初期化用の処理
        /// </summary>
        /// <param name="stateType">最初に設定したいStateのType</param>
        public void Initialize(T stateType);
        
        /// <summary>
        /// 毎フレーム実行する処理
        /// </summary>
        public void MyUpdate();

        /// <summary>
        /// 等間隔で実行する処理
        /// </summary>
        public void MyFixedUpdate();
    }
}
