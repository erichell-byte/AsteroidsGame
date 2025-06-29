using Config;
using MovementBehavior;
using UnityEngine;

namespace Enemies
{
    public class AsteroidEnemy : Enemy
    {
        private IMovementBehavior movementBehavior;

        public override void Initialize(EnemyConfig config)
        {
            base.Initialize(config);
            movementBehavior = new RandomMovementBehavior(rb);
            movementBehavior.Move(config);
        }

        public override void SetActive(bool isActive)
        {
            Debug.Log($"AsteroidEnemy.SetActive: {isActive}");
            if (isActive)
            {
                movementBehavior.ResumeMove();
                movementBehavior.Move(config);
            }
            else
            {
                movementBehavior.StopMove();
            }
            base.SetActive(isActive);
        }
    }
}