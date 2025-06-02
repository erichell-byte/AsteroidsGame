using Config;
using UnityEngine;

namespace MovementBehavior
{
    public class ChaseMovementBehavior : IMovementBehavior
    {
        private readonly Transform target;

        public ChaseMovementBehavior(Transform target)
        {
            this.target = target;
        }

        public void Move(Rigidbody2D rigidbody, EnemyConfig config)
        {
            if (rigidbody == null || target == null) return;

            Vector2 direction = (target.position - rigidbody.transform.position).normalized;
            rigidbody.linearVelocity = direction * config.speed;
        }
    }
}