using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
        public AssetReferenceGameObject bulletId;

        [Space(20)]
        [Header("Enemies Configuration")]
        public float asteroidSpawnFrequency;
        public float ufoSpawnFrequency;
        public AssetReferenceGameObject asteroidId;
        public AssetReferenceGameObject asteroidSmallId;
        public AssetReferenceGameObject ufoId;
        public List<EnemyConfig> enemiesConfigs;
    }
}