using Config;
using MovementBehavior;
using UnityEngine;

namespace Enemies
{
    public class UFOEnemy : Enemy
    {
        private IMovementBehavior movementBehavior;

        public void SetTarget(Transform target)
        {
            movementBehavior = new ChaseMovementBehavior(target);
        }

        public override void Initialize(EnemyConfig config)
        {
            base.Initialize(config);
            if (movementBehavior != null)
            {
                movementBehavior.Move(rb, config);
            }
        }

        private void Update()
        {
            if (movementBehavior != null)
            {
                movementBehavior.Move(rb, config);
            }
        }
    }
}