using Config;
using MovementBehavior;

namespace Enemies
{
    public class AsteroidEnemy : Enemy
    {
        private IMovementBehavior movementBehavior;

        public override void Initialize(EnemyConfig config)
        {
            base.Initialize(config);
            movementBehavior = new RandomMovementBehavior();
            movementBehavior.Move(rb, config);
        }
    }
}