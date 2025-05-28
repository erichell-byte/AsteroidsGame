using UniRx;
using UnityEngine;

namespace Character
{
    public class CharacterModel
    {
        public ReactiveProperty<Vector2> Position { get; } = new();
        public ReactiveProperty<float> Rotation { get; } = new();
        public ReactiveProperty<float> Speed { get; } = new();
        public ReactiveProperty<int> LaserCount { get; } = new();
        public ReactiveProperty<float> TimeToRecoveryLaser { get; } = new();
        public ReactiveProperty<bool> IsDead { get; } = new();

        public CharacterModel(Vector2 position,
            float rotation,
            float speed,
            int laserCount,
            float timeToRecoveryLaser)
        {
            Position.Value = position;
            Rotation.Value = rotation;
            Speed.Value = speed;
            LaserCount.Value = laserCount;
            TimeToRecoveryLaser.Value = timeToRecoveryLaser;
            IsDead.Value = false;
        }
        
        public void SetPosition(Vector2 pos) => Position.Value = pos;
        public void SetRotation(float rot) => Rotation.Value = rot;
        public void SetSpeed(float speed) => Speed.Value = speed;
        public void SetLaserCount(int count) => LaserCount.Value = count;
        public void SetTimeToRecoveryLaser(float time) => TimeToRecoveryLaser.Value = time;
        public void SetIsDead(bool isDead) => IsDead.Value = isDead;
    }
}
