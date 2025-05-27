using Pools;
using Systems;
using UnityEngine;
using Weapon;
using Zenject;

namespace Components
{
    public class AttackComponent : MonoBehaviour, IGameUpdateListener
    {
        [SerializeField] private Transform shotPoint;
        [SerializeField] private GameObject laser;
        [SerializeField] private LayerMask laserLayerMask;

        private MainWeapon mainWeapon;
        private LaserWeapon laserWeapon;
        private DiContainer container;

        [Inject]
        private void Construct(DiContainer container)
        {
            this.container = container;
        }

        public void Initialize(
            int countOfLaserShots,
            float bulletSpeed,
            Bullet bulletPrefab,
            Transform bulletPoolParent)
        {
            var bulletPool = new BulletPoolFacade(bulletPrefab, bulletPoolParent);

            mainWeapon = container.Instantiate<MainWeapon>();
            mainWeapon.Initialize(shotPoint, bulletSpeed, bulletPool);
            laserWeapon = container.Instantiate<LaserWeapon>();
            laserWeapon.Initialize(
                shotPoint,
                laser,
                laserLayerMask,
                countOfLaserShots);
        }

        public void AttackByMainShot()
        {
            mainWeapon.Attack();
        }

        public void AttackByLaserShot()
        {
            laserWeapon.Attack();
        }

        public void OnUpdate(float deltaTime)
        {
            laserWeapon.Update();
        }

        public void ResetWeapon()
        {
            laserWeapon.TurnOffLaser();
            mainWeapon.Reset();
            mainWeapon = null;
            laserWeapon = null;
        }

        public LaserWeapon GetLaserWeapon()
        {
            return laserWeapon;
        }
    }
}

