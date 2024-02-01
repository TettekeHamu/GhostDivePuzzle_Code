using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle.Refactoring
{
    /// <summary>
    /// 攻撃できるダイブオブジェクトのコンポーネントを管理する子クラス
    /// </summary>
    public class AttackableObjectComponentController : DiveObjectComponentController
    {
        /// <summary>
        /// 電源がついてる時に使用する用のParticle
        /// </summary>
        [SerializeField] private ParticleSystem lightningParticle;
        /// <summary>
        /// 電源をつけているかどうか
        /// </summary>
        private bool isRightOn;
        /// <summary>
        /// 電力を使い果たしたかどうか
        /// </summary>
        private bool isRunningOutPower;
        
        public bool IsRunningOutPower => isRunningOutPower;
        
        /// <summary>
        /// 初期化処理
        /// </summary>
        private void Awake()
        {
            isRightOn = false;
            lightningParticle.gameObject.SetActive(false);
            isRunningOutPower = true;
        }

        /// <summary>
        /// 電源のオンオフを切り替える処理
        /// </summary>
        /// <returns></returns>
        public bool ChangeRight(bool isInPlayer = true)
        {
            if (isInPlayer)
            {
                if(!isRunningOutPower) return false;
                isRightOn = !isRightOn;
                lightningParticle.gameObject.SetActive(isRightOn);                
            }
            else
            {
                isRightOn = false;
                lightningParticle.gameObject.SetActive(false);   
            }

            return isRightOn;
        }

        /// <summary>
        /// 電源を切る処理
        /// </summary>
        public void RunOutPower()
        {
            isRunningOutPower = false;
            lightningParticle.gameObject.SetActive(false);
        }  
    }
}