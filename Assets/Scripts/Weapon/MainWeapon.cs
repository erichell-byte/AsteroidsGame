using GameSystem;
using Pools;
using UnityEngine;

namespace Weapon
{
    public class MainWeapon : BaseWeapon
    {
        private readonly Timer shotTimer;
        private readonly float bulletSpeed;
        private BulletPoolFacade bulletPool;

        public MainWeapon(Transform shotPoint, float bulletSpeed, BulletPoolFacade bulletPool, float shotFrequency) :
            base(shotPoint)
        {
            this.bulletSpeed = bulletSpeed;
            this.bulletPool = bulletPool;
            shotTimer = new Timer(shotFrequency);
        }

        public override void Attack()
        {
            if (shotTimer.IsPlaying()) return;

            Shot();
            shotTimer.Play();
        }

        private void Shot()
        {
            var bullet = bulletPool.Pool.Get();
            bullet.OnHit += OnHitBullet;
            bullet.transform.position = ShotPoint.position;
            bullet.transform.rotation = ShotPoint.rotation;
            bullet.GetComponent<Rigidbody2D>().velocity = ShotPoint.up * bulletSpeed;
        }

        private void OnHitBullet(Bullet bullet)
        {
            bullet.OnHit -= OnHitBullet;
            bulletPool.Pool.Release(bullet);
        }

        public void Reset()
        {
            bulletPool.Pool.Clear();
        }
    }
}