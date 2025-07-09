using Enemies;
using Systems;
using UniRx;
using UnityEngine;
using Zenject;

namespace Components
{
    public class CollisionComponent : MonoBehaviour
    {
        private IGameEvents gameEvents;

        [Inject]
        private void Construct(IGameEvents gameEvents)
        {
            this.gameEvents = gameEvents;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Enemy>())
            {
                gameEvents.NotifySpaceshipCollidedWithEnemy();
            }
        }
    }
}