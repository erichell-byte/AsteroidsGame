using System;
using UnityEngine;

namespace Config
{
    [Serializable]
    public class RemoteConfig
    {
        [Header("Character Configuration")]
        public float moveCoefficient;
        public float rotateCoefficient;
        public float maxVelocityMagnitude;
        
        [Header("Attack Configuration")]
        public float bulletSpeed;
        public float shotFrequency;
        public float timeToRecoveryLaser;
        public float timeToDurationLaser;
        public int countOfLaserShots;
        
        [Header("Enemies Configuration")]
        public float asteroidSpawnFrequency;
        public float ufoSpawnFrequency;
    }
}