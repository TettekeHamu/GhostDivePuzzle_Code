using System;
namespace TettekeKobo.StateMachine
{
    /// <summary>
    /// Stateを変更する処理を持つInterface
    /// </summary>
    /// <typeparam name="T">変更したいStateを表すEnum</typeparam>
    public interface ITransitionState<in T> where T : Enum
    {
        /// <summary>
        /// Stateを変更する処理
        /// </summary>
        /// <param name="stateType">変更したいStateを表すEnum</param>
        public void TransitionState(T stateType);
    }
}
