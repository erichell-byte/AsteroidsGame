using System;
using Pools;
using UnityEngine;
using Utils;
using Zenject;

namespace Weapon
{
	public class MainWeapon : BaseWeapon
	{
		private BulletPoolFacade _bulletPool;
		private float _bulletSpeed;
		private TimersController _timersController;


		public Action OnShot;

		[Inject]
		private void Construct(TimersController timersController)
		{
			_timersController = timersController;
		}

		public void Initialize(
			Transform shotPoint,
			float bulletSpeed,
			BulletPoolFacade bulletPool)
		{
			this.shotPoint = shotPoint;
			_bulletSpeed = bulletSpeed;
			_bulletPool = bulletPool;
			_timersController.InitMainWeaponTimer();
		}

		public override void Attack()
		{
			if (_timersController.MainWeaponTimerIsPlaying()) return;
			Shot();
			_timersController.PlayMainWeaponTimer();
		}

		private async void Shot()
		{
			OnShot?.Invoke();

			var bullet = await _bulletPool.GetAsync();
			bullet.OnHit += OnHitBullet;
			bullet.transform.position = shotPoint.position;
			bullet.transform.rotation = shotPoint.rotation;
			bullet.GetComponent<Rigidbody2D>().linearVelocity = shotPoint.up * _bulletSpeed;
		}

		private void OnHitBullet(Bullet bullet)
		{
			bullet.OnHit -= OnHitBullet;
			_bulletPool.Release(bullet);
		}

		public void Reset()
		{
			_bulletPool.Clear();
		}
	}
}