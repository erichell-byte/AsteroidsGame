using Enemies;
using UnityEngine;
using Weapon;

namespace Config
{
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "ScriptableObjects/GameConfiguration", order = 0)]
    public class GameConfiguration : ScriptableObject
    {
        [Header("Character Configuration")]
        public float moveCoefficient;

        public float rotateCoefficient;
        public float maxVelocityMagnitude;

        [Space(20)]
        [Header("Attack Configuration")]
        public float bulletSpeed;

        public float shotFrequency;
        public float timeToRecoveryLaser;
        public float timeToDurationLaser;
        public int countOfLaserShots;
        public Bullet bulletPrefab;

        [Space(20)]
        [Header("Enemies Configuration")]
        public float asteroidSpawnFrequency;

        public float ufoSpawnFrequency;
        public AsteroidBigEnemy asteroidBigPrefab;
        public AsteroidSmallEnemy asteroidSmallPrefab;
        public UFOEnemy ufoPrefab;

        public EnemyConfig asteroidBigConfig;
        public EnemyConfig asteroidSmallConfig;
        public EnemyConfig ufoConfig;
    }
}