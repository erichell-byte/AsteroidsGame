using Config;
using MovementBehavior;

namespace Enemies
{
    public class AsteroidEnemy : Enemy
    {
        private IMovementBehavior movementBehavior;
        private bool shouldDestroyOnSmall = true;

        public override void Initialize(EnemyConfig config)
        {
            base.Initialize(config);
            movementBehavior = new RandomMovementBehavior();
            movementBehavior.Move(rb, config);
        }
        
        public void DoAsteroidSmall()
        {
            shouldDestroyOnSmall = false;
            transform.localScale = new UnityEngine.Vector3(0.5f, 0.5f, 0.5f);
        }
        
        public bool IsShouldDestroyOnSmall()
        {
            return shouldDestroyOnSmall;
        }

        public void ResetAsteroid()
        {
            shouldDestroyOnSmall = true;
            transform.localScale = UnityEngine.Vector3.one;
        }
    }
}