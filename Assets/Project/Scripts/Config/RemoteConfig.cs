using System;
using UnityEngine;

namespace Config
{
    [Serializable]
    public class RemoteConfig
    {
        [Header("Character Configuration")]
        public float MoveCoefficient;
        public float RotateCoefficient;
        public float MaxVelocityMagnitude;
        
        [Header("Attack Configuration")]
        public float BulletSpeed;
        public float ShotFrequency;
        public float TimeToRecoveryLaser;
        public float TimeToDurationLaser;
        public int CountOfLaserShots;
        
        [Header("Enemies Configuration")]
        public float AsteroidSpawnFrequency;
        public float UfoSpawnFrequency;
    }
}