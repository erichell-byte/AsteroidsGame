using System;

namespace Config
{
    [Serializable]
    public class RemoteConfig
    {
        public float moveCoefficient;
        public float rotateCoefficient;
        public float maxVelocityMagnitude;
        public float bulletSpeed;
        public float shotFrequency;
        public float timeToRecoveryLaser;
        public float timeToDurationLaser;
        public int countOfLaserShots;
        public float asteroidSpawnFrequency;
        public float ufoSpawnFrequency;
    }
}