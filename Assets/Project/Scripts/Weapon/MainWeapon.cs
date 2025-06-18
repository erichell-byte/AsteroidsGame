using System;
using Pools;
using UnityEngine;
using Utils;
using Zenject;

namespace Weapon
{
    public class MainWeapon : BaseWeapon
    {
        private TimersController timersController;
        private float bulletSpeed;
        private BulletPoolFacade bulletPool;

        public Action OnShot;

        [Inject]
        private void Construct(TimersController timersController)
        {
            this.timersController = timersController;
        }

        public void Initialize(
            Transform shotPoint,
            float bulletSpeed,
            BulletPoolFacade bulletPool)
        {
            this.shotPoint = shotPoint;
            this.bulletSpeed = bulletSpeed;
            this.bulletPool = bulletPool;
            timersController.InitMainWeaponTimer();
        }

        public override void Attack()
        {
            if (timersController.MainWeaponTimerIsPlaying()) return;
            Shot();
            timersController.PlayMainWeaponTimer();
        }

        private void Shot()
        {
            OnShot?.Invoke();
            var bullet = bulletPool.GetAsync().Result;
            bullet.OnHit += OnHitBullet;
            bullet.transform.position = shotPoint.position;
            bullet.transform.rotation = shotPoint.rotation;
            bullet.GetComponent<Rigidbody2D>().linearVelocity = shotPoint.up * bulletSpeed;
        }

        private void OnHitBullet(Bullet bullet)
        {
            bullet.OnHit -= OnHitBullet;
            bulletPool.Release(bullet);
        }

        public void Reset()
        {
            bulletPool.Clear();
        }
    }
}