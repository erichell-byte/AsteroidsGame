using Config;
using UnityEngine;

namespace MovementBehavior
{
	public class ChaseMovementBehavior : IMovementBehavior
	{
		private readonly Rigidbody2D _rigidbody;
		private readonly Transform _target;

		public ChaseMovementBehavior(Transform target, Rigidbody2D rigidbody)
		{
			_target = target;
			_rigidbody = rigidbody;
			ResumeMove();
		}

		public bool IsMove { get; set; }

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