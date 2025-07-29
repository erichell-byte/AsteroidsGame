using Config;
using UnityEngine;

namespace MovementBehavior
{
    public class ChaseMovementBehavior : IMovementBehavior
    {
        private readonly Transform _target;
        private readonly Rigidbody2D _rigidbody;

        public bool IsMove { get; set; }
        
        public ChaseMovementBehavior(Transform target, Rigidbody2D rigidbody)
        {
            this._target = target;
            this._rigidbody = rigidbody;
            ResumeMove();
        }
        
        public void Move(EnemyConfig config)
        {
            if (_rigidbody == null || _target == null || IsMove == false) return;

            Vector2 direction = (_target.position - _rigidbody.transform.position).normalized;
            _rigidbody.linearVelocity = direction * config.Speed;
        }

        public void StopMove()
        {
            IsMove = false;
            _rigidbody.linearVelocity = Vector2.zero;
        }
        
        public void ResumeMove()
        {
            IsMove = true;
        }
    }
}