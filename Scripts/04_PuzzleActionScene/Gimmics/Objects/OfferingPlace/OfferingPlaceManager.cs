using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// オソナエサキにアタッチするクラス
    /// </summary>
    public class OfferingPlaceManager : MonoBehaviour
    {
        /// <summary>
        /// お供えされたことを伝えるSubject
        /// </summary>
        private readonly Subject<Unit> onCompleteSubject = new Subject<Unit>();
        /// <summary>
        /// お供えされたことを伝えるObservable
        /// </summary>
        public IObservable<Unit> OnCompleteObservable => onCompleteSubject;
        
        /// <summary>
        /// オハカがオソナエされたときにおこなうメソッド
        /// </summary>
        public void SetOfferingObject()
        {
            onCompleteSubject.OnNext(Unit.Default);
        }
    }
}
