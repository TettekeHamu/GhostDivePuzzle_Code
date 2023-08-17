using System.Collections;
using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// プレイヤーのパーティクルを管理するクラス
    /// </summary>
    public class PlayerParticleManager : MonoBehaviour
    {
        /// <summary>
        /// 地面とぶつかったときのパーティクル
        /// </summary>
        [SerializeField] private ParticleSystem collisionGroundParticlePrefab;

        /// <summary>
        /// 地面とぶつかった際にパーティクルを生成する処理
        /// </summary>
        public void PlayCollisionGroundParticle()
        {
            StartCoroutine(ParticleCoroutine());
        }

        /// <summary>
        /// パーティクルを生成して、再生後に破棄するコルーチン
        /// </summary>
        private IEnumerator ParticleCoroutine()
        {
            var timeLength = collisionGroundParticlePrefab.main.startLifetimeMultiplier;
            var particleObject = Instantiate(collisionGroundParticlePrefab, transform.position + Vector3.down * 0.5f, Quaternion.identity);
            particleObject.Play();
            yield return new WaitForSeconds(timeLength);
            Destroy(particleObject.gameObject);
        }
    }
}
