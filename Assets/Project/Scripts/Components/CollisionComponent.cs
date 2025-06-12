using Character;
using Enemies;
using UnityEngine;
using Zenject;

namespace Components
{
    public class CollisionComponent : MonoBehaviour
    {
        private SpaceshipController spaceshipController;

        [Inject]
        private void Construct(SpaceshipController spaceshipController)
        {
            this.spaceshipController = spaceshipController;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Enemy>())
            {
                spaceshipController.SpaceshipModel.SetIsDead(true);
            }
        }
    }
}