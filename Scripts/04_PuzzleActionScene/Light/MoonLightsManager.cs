using System;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// Scene内のツキアカリをまとめて管理するクラス
    /// </summary>
    public class MoonLightsManager : MonoBehaviour
    {
        /// <summary>
        /// シーン内にあるツキアカリ
        /// </summary>
        private MoonLight[] moonLights;

        private void Awake()
        {
            moonLights = FindObjectsOfType<MoonLight>();
        }

        private void Update()
        {
            foreach (var moon in moonLights)
            {
                moon.MyUpdate();
            }
        }
    }
}
