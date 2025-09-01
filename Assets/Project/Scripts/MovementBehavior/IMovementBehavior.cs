using Config;

namespace MovementBehavior
{
	public interface IMovementBehavior
	{
		public bool IsMove { get; set; }

		void Move(EnemyConfig config);

		void StopMove();

		void ResumeMove();
	}
}