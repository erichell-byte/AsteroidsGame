using Config;
using MovementBehavior;
using UnityEngine;

namespace Enemies
{
    public class UFOEnemy : Enemy
    {
        private IMovementBehavior movementBehavior;

        public override void Initialize(EnemyConfig config)
        { 
            base.Initialize(config);
            if (movementBehavior != null)
            {
                movementBehavior.Move(config);
            }
        }
        
        public void InitMovement(Transform target)
        {
            movementBehavior = new ChaseMovementBehavior(target, rb);
        }

        public override void SetActive(bool isActive)
        {
            if (isActive)
                movementBehavior.ResumeMove();
            else
                movementBehavior.StopMove();
            
            base.SetActive(isActive);
        }

        private void Update()
        {
            if (movementBehavior != null)
            {
                movementBehavior.Move(config);
            }
        }
    }
}