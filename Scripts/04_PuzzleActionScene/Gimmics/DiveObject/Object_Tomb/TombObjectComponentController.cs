using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle.Refactoring
{
    /// <summary>
    /// お墓のオブジェクトのコンポーネントを管理する子クラス
    /// </summary>
    public class TombObjectComponentController : DiveObjectComponentController
    {
        /// <summary>
        /// オソナエをおこなう機能をもったクラス
        /// </summary>
        [SerializeField] private OfferingManager offeringManager;

        public OfferingManager OfferingManager => offeringManager;
    }
}