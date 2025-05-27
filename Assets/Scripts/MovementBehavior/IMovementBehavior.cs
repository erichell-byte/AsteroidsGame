using Config;
using UnityEngine;

namespace MovementBehavior
{
    public interface IMovementBehavior
    {
        void Move(Rigidbody2D rigidbody, EnemyConfig config);
    }
}