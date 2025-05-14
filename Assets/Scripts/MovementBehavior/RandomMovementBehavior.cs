using Config;
using Enemies;
using UnityEngine;

namespace MovementBehavior
{
    public class RandomMovementBehavior : IMovementBehavior
    {
        public void Move(Rigidbody2D rigidbody, EnemyConfig config)
        {
            if (rigidbody == null) return;

            float speed = config.Speed * config.SpeedModifier;
            float randomAngle = Random.Range(0f, 360f);
            Vector2 direction = new Vector2(
                Mathf.Cos(randomAngle * Mathf.Deg2Rad),
                Mathf.Sin(randomAngle * Mathf.Deg2Rad)
            );
            rigidbody.velocity = direction * speed;
        }
    }
}