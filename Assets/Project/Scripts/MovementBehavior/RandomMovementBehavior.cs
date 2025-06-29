using Config;
using UnityEngine;

namespace MovementBehavior
{
    public class RandomMovementBehavior : IMovementBehavior
    {
        private Rigidbody2D rigidbody;
        public bool IsMove { get; set; }

        public RandomMovementBehavior(Rigidbody2D rigidbody)
        {
            this.rigidbody = rigidbody;
            ResumeMove();
        }
        
        public void Move(EnemyConfig config)
        {
            if (rigidbody == null || IsMove == false) return;
            
            float speed = config.speed * config.speedModifier;
            float randomAngle = Random.Range(0f, 360f);
            Vector2 direction = new Vector2(
                Mathf.Cos(randomAngle * Mathf.Deg2Rad),
                Mathf.Sin(randomAngle * Mathf.Deg2Rad)
            );
            rigidbody.linearVelocity = direction * speed;
        }
        
        public void StopMove()
        {
            rigidbody.linearVelocity = Vector3.zero;
            IsMove = false;
        }
        
        public void ResumeMove()
        {
            IsMove = true;
        }
        
        
    }
}