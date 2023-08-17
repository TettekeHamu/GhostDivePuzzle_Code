using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// ツキアカリを管理するクラス
    /// </summary>
    public class MoonLight : MonoBehaviour
    {
        /// <summary>
        /// Light2Dコンポーネント
        /// </summary>
        [SerializeField] private Light2D light2D;
        /// <summary>
        /// Inspectorから設定してるIntensity（光量）
        /// </summary>
        private float startIntensity;
        /// <summary>
        /// 光量の遷移させる用の乱数
        /// </summary>
        private float randomStartNum;

        /// <summary>
        /// 
        /// </summary>
        public Light2D Light2D => light2D;
        /// <summary>
        /// 
        /// </summary>
        public float StartIntensity => startIntensity;

        private void Awake()
        {
            startIntensity = light2D.intensity;
            randomStartNum = Random.Range(0, Mathf.PI / 2);
        }

        public void MyUpdate()
        {
            light2D.intensity = startIntensity * Mathf.Abs(Mathf.Cos(Time.time * 0.2f + randomStartNum));
        }
    }
}
