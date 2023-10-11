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
        /// プレイヤーを追従するパーティクル
        /// </summary>
        [SerializeField] private ParticleSystem trailParticlePrefab;
        /// <summary>
        /// 地面とぶつかったときのパーティクル
        /// </summary>
        [SerializeField] private ParticleSystem collisionGroundParticlePrefab;
        /// <summary>
        /// やられた際のパーティクル
        /// </summary>
        [SerializeField] private ParticleSystem deadParticlePrefab;
        /// <summary>
        /// プレイヤーに追従させるパーティクル
        /// </summary>
        private ParticleSystem trailParticleObject;

        /// <summary>
        /// プレイヤーに追従するパーティクルを生成する処理
        /// </summary>
        public void PlayTrailParticle()
        {
            trailParticleObject = Instantiate(trailParticlePrefab, transform.position, Quaternion.identity);
            trailParticleObject.Play();
        }

        /// <summary>
        /// プレイヤーにパーティクルを追従させる処理
        /// </summary>
        /// <param name="playerPos"></param>
        public void FollowPlayer(Vector3 playerPos)
        {
            trailParticleObject.transform.position = playerPos;
        }

        /// <summary>
        /// 地面とぶつかった際にパーティクルを生成する処理
        /// </summary>
        public void PlayCollisionGroundParticle()
        {
            StartCoroutine(CollisionGroundParticleCoroutine());
        }

        /// <summary>
        /// パーティクルを生成して、再生後に破棄するコルーチン
        /// </summary>
        private IEnumerator CollisionGroundParticleCoroutine()
        {
            var timeLength = collisionGroundParticlePrefab.main.startLifetimeMultiplier;
            var particleObject = Instantiate(collisionGroundParticlePrefab, transform.position + Vector3.down * 0.5f, Quaternion.identity);
            particleObject.Play();
            yield return new WaitForSeconds(timeLength);
            Destroy(particleObject.gameObject);
        }

        public float PlayDeadParticle()
        {
            var timeLength = deadParticlePrefab.main.startLifetimeMultiplier;
            var deadParticle = Instantiate(deadParticlePrefab, transform.position, Quaternion.identity);
            deadParticle.Play();
            return timeLength;
        }
    }
}
