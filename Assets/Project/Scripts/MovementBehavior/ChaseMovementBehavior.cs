using Config;
using UnityEngine;

namespace MovementBehavior
{
    public class ChaseMovementBehavior : IMovementBehavior
    {
        private readonly Transform target;
        private Rigidbody2D rigidbody;

        public bool IsMove { get; set; }
        
        public ChaseMovementBehavior(Transform target, Rigidbody2D rigidbody)
        {
            this.target = target;
            this.rigidbody = rigidbody;
            ResumeMove();
        }
        
        public void Move(EnemyConfig config)
        {
            if (rigidbody == null || target == null || IsMove == false) return;

            Vector2 direction = (target.position - rigidbody.transform.position).normalized;
            rigidbody.linearVelocity = direction * config.speed;
        }

        public void StopMove()
        {
            IsMove = false;
            rigidbody.linearVelocity = Vector2.zero;
        }
        
        public void ResumeMove()
        {
            IsMove = true;
        }
    }
}