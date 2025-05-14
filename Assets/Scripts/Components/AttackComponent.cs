using Pools;
using Systems;
using UnityEngine;
using Weapon;

namespace Components
{
    public class AttackComponent : MonoBehaviour, IGameUpdateListener
    {
        [SerializeField] private Transform shotPoint;
        [SerializeField] private GameObject laser;
        [SerializeField] private LayerMask laserLayerMask;

        private MainWeapon mainWeapon;
        private LaserWeapon laserWeapon;

        public void Initialize(
            float shotFrequency,
            float timeToRecoveryLaser,
            float timeToDurationLaser,
            int countOfLaserShots,
            float bulletSpeed,
            Bullet bulletPrefab,
            Transform bulletPoolParent)
        {
            var bulletPool = new BulletPoolFacade(bulletPrefab, bulletPoolParent);

            mainWeapon = new MainWeapon(shotPoint, bulletSpeed, bulletPool, shotFrequency);
            laserWeapon = new LaserWeapon(
                shotPoint,
                laser,
                laserLayerMask,
                timeToRecoveryLaser,
                timeToDurationLaser,
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