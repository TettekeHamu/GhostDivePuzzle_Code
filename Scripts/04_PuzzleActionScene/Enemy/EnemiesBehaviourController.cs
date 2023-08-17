using UnityEngine;

namespace TettekeKobo.GhostDivePuzzle
{
    /// <summary>
    /// シーン内の敵を管理するクラス
    /// </summary>
    public class EnemiesBehaviourController : MonoBehaviour
    {
        /// <summary>
        /// 鳥の敵
        /// </summary>
        private EnemyBardManager[] enemyBards;
        /// <summary>
        /// 目の敵
        /// </summary>
        private EnemyEyeManager[] enemyEyes;
        
        private void EnemyBardInitialize()
        {
            enemyBards = FindObjectsOfType<EnemyBardManager>();
            foreach (var enemy in enemyBards)
            {
                enemy.Initialize();
            }
        }

        private void EnemyBardUpdate()
        {
            foreach (var enemy in enemyBards)
            {
                enemy.MyUpDate();
            }
        }

        private void EnemyEyeInitialize()
        {
            enemyEyes = FindObjectsOfType<EnemyEyeManager>();
            foreach (var enemy in enemyEyes)
            {
                enemy.Initialize();
            }
        }

        private void EnemyEyeUpdate()
        {
            foreach (var enemy in enemyEyes)
            {
                enemy.MyUpDate();
            }
        }
        
        /// <summary>
        /// 敵をすべて初期化するメソッド
        /// </summary>
        public void InitializeEnemies()
        {
            EnemyBardInitialize();
            EnemyEyeInitialize();
        }

        public void MyUpdate()
        {
            EnemyBardUpdate();
            EnemyEyeUpdate();
        }
    }
}
