using Config;
using Enemies;
using UnityEngine;

namespace MovementBehavior
{
    public interface IMovementBehavior
    {
        void Move(Rigidbody2D rigidbody, EnemyConfig config);
    }
}