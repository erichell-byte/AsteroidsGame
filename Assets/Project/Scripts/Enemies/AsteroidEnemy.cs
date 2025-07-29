using Config;
using MovementBehavior;

namespace Enemies
{
    public class AsteroidEnemy : Enemy
    {
        private IMovementBehavior _movementBehavior;

        public override void Initialize(EnemyConfig config)
        {
            base.Initialize(config);
            _movementBehavior = new RandomMovementBehavior(Rb);
            _movementBehavior.Move(config);
        }

        public override void SetActive(bool isActive)
        {
            if (isActive)
            {
                _movementBehavior.ResumeMove();
                _movementBehavior.Move(Config);
            }
            else
            {
                _movementBehavior.StopMove();
            }
            base.SetActive(isActive);
        }
    }
}