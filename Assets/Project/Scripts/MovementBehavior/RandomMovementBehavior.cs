using Config;
using UnityEngine;

namespace MovementBehavior
{
    public class RandomMovementBehavior : IMovementBehavior
    {
        private readonly Rigidbody2D _rigidbody;
        public bool IsMove { get; set; }

        public RandomMovementBehavior(Rigidbody2D rigidbody)
        {
            this._rigidbody = rigidbody;
            ResumeMove();
        }
        
        public void Move(EnemyConfig config)
        {
            if (_rigidbody == null || IsMove == false) return;
            
            float speed = config.Speed * config.SpeedModifier;
            float randomAngle = Random.Range(0f, 360f);
            Vector2 direction = new Vector2(
                Mathf.Cos(randomAngle * Mathf.Deg2Rad),
                Mathf.Sin(randomAngle * Mathf.Deg2Rad)
            );
            _rigidbody.linearVelocity = direction * speed;
        }
        
        public void StopMove()
        {
            _rigidbody.linearVelocity = Vector3.zero;
            IsMove = false;
        }
        
        public void ResumeMove()
        {
            IsMove = true;
        }
        
        
    }
}