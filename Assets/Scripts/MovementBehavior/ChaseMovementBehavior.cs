using Config;
using Enemies;
using UnityEngine;

namespace MovementBehavior
{
    public class ChaseMovementBehavior : IMovementBehavior
    {
        private readonly Transform _target;

        public ChaseMovementBehavior(Transform target)
        {
            _target = target;
        }

        public void Move(Rigidbody2D rigidbody, EnemyConfig config)
        {
            if (rigidbody == null || _target == null) return;

            Vector2 direction = (_target.position - rigidbody.transform.position).normalized;
            rigidbody.velocity = direction * config.Speed;
        }
    }
}