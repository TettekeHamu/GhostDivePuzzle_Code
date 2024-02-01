using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// オハカのオソナエの機能を持つクラス
    /// </summary>
    public class OfferingManager : MonoBehaviour
    {
        /// <summary>
        /// オハカのアニメーター
        /// </summary>
        [SerializeField] private Animator animator;
        /// <summary>
        /// ゴールした際のアニメーショントランジションのint値
        /// </summary>
        private readonly int isOffering = Animator.StringToHash("IsOffering");
        /// <summary>
        /// 位置が一致したとみなす距離
        /// </summary>
        private float matchDistance;
        /// <summary>
        /// シーン中にあるオソナエサキの配列
        /// </summary>
        private OfferingPlaceManager[] offeringPlaces;
        /// <summary>
        /// 一番近くにあるオソナエサキ
        /// </summary>
        private OfferingPlaceManager nearestOfferingPlace;
        /// <summary>
        /// すでにオソナエされたオソナエサキ
        /// </summary>
        private OfferingPlaceManager completeOfferingPlace;
        /// <summary>
        /// オハカがオソナエサキにあるかないか
        /// </summary>
        private bool isCompletedOffering;

        public OfferingPlaceManager NearestOfferingPlace => nearestOfferingPlace;
        public bool IsCompletedOffering => isCompletedOffering;

        private void Awake()
        {
            matchDistance = 0.1f;
            offeringPlaces = FindObjectsOfType<OfferingPlaceManager>();
            nearestOfferingPlace = null;
            completeOfferingPlace = null;
            isCompletedOffering = false;
        }
        
        /// <summary>
        /// 一番近いターゲットとの距離を測る処理
        /// </summary>
        /// <returns>近くにあればtrueを返す</returns>
        public bool CheckNearestOfferingPosDistance()
        {
            //一番近い目的地との距離を算出する
            float minDistance = float.MaxValue;
            foreach (var offeringPlace in offeringPlaces)
            {
                float distance = (transform.position - offeringPlace.transform.position).magnitude;
                if (minDistance > distance)
                {
                    minDistance = distance;
                    //一番近い位置にあるOfferingPlaceを格納
                    nearestOfferingPlace = offeringPlace;
                }
            }

            //距離がかなり近い＆お供えされていない場合trueを返すようにする
            if (minDistance < matchDistance && !completeOfferingPlace)
            {
                //お供えされたことにする
                isCompletedOffering = true;
                completeOfferingPlace = nearestOfferingPlace;
                animator.SetBool(isOffering,true);
                //ターゲット側に通知
                //targetPosObject.SetOfferingObject();
                return true;
            }
            
            //近くにTargetがなかったり、あっても憑依されていたらfalseを返す
            return false;
        } 
    }
}
