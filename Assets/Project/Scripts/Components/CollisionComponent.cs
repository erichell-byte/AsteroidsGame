using Enemies;
using Systems;
using UnityEngine;
using Zenject;

namespace Components
{
    public class CollisionComponent : MonoBehaviour
    {
        private IGameEvents _gameEvents;

        [Inject]
        private void Construct(IGameEvents gameEvents)
        {
            this._gameEvents = gameEvents;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Enemy>())
            {
                _gameEvents.NotifySpaceshipCollidedWithEnemy();
            }
        }
    }
}