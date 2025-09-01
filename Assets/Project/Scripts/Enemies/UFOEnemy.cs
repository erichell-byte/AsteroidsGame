using Config;
using MovementBehavior;
using UnityEngine;

namespace Enemies
{
	public class UFOEnemy : Enemy
	{
		private IMovementBehavior _movementBehavior;

		private void Update()
		{
			if (_movementBehavior != null) _movementBehavior.Move(Config);
		}

		public override void Initialize(EnemyConfig config)
		{
			base.Initialize(config);
			if (_movementBehavior != null) _movementBehavior.Move(config);
		}

		public void InitMovement(Transform target)
		{
			_movementBehavior = new ChaseMovementBehavior(target, Rb);
		}

		public override void SetActive(bool isActive)
		{
			if (isActive)
				_movementBehavior.ResumeMove();
			else
				_movementBehavior.StopMove();

			base.SetActive(isActive);
		}
	}
}