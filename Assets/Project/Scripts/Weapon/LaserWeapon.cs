using System;
using Enemies;
using UniRx;
using UnityEngine;
using Utils;
using Zenject;

namespace Weapon
{
	public class LaserWeapon : BaseWeapon, IDisposable
	{
		private bool _disposed;
		private bool _isActive;
		private bool _isRecovering;
		private GameObject _laser;
		private LayerMask _layerMask;
		private int _maxShots;
		private TimersController _timersController;

		public Action OnLaserShot;

		public ReactiveProperty<int> RemainingShots { get; } = new();
		public ReactiveProperty<float> TimeToRecovery { get; } = new();

		[Inject]
		private void Construct(
			TimersController timersController)
		{
			_timersController = timersController;
		}

		public void Dispose()
		{
			if (_disposed) return;
			_disposed = true;
			_timersController.UnsubscribeAllLaserTimers(ChangedTimeToRecovery, TurnOffLaser, RecoveryLaser);
		}

		public void Initialize(
			Transform shotPoint,
			GameObject laser,
			LayerMask layerMask,
			int maxShots)
		{
			this.ShotPoint = shotPoint;
			this._laser = laser;
			this._layerMask = layerMask;
			this._maxShots = maxShots;
			RemainingShots.Value = maxShots;
			TimeToRecovery.Value = 0f;
			_timersController.InitLaserTimers(ChangedTimeToRecovery);
		}

		public override void Attack()
		{
			if (RemainingShots.Value <= 0 || _isActive) return;

			RemainingShots.Value--;
			_isActive = true;
			_laser.SetActive(true);
			_timersController.PlayAndSubscribeDurationTimer(TurnOffLaser);
			OnLaserShot?.Invoke();
		}

		public void Tick()
		{
			HandleRecovery();
			HandleLaserDamage();
		}

		private void HandleRecovery()
		{
			if (_timersController.RecoveryTimerIsPlaying() == false && RemainingShots.Value < _maxShots &&
			    !_isRecovering)
			{
				_isRecovering = true;
				_timersController.PlayAndSubscribeRecoveryTimer(RecoveryLaser);
			}
		}

		private void HandleLaserDamage()
		{
			if (_isActive == false) return;

			var hit = Physics2D.Raycast(ShotPoint.position, ShotPoint.up, float.PositiveInfinity, _layerMask);

			if (hit.collider == null) return;
			if (hit.collider.TryGetComponent(out Enemy enemy)) enemy.Die();
		}

		public void TurnOffLaser()
		{
			_timersController.UnsubscribeFromDurationTimer(TurnOffLaser);
			_isActive = false;
			_laser.SetActive(false);
		}

		private void RecoveryLaser()
		{
			_timersController.UnsubscribeFromRecoveryTimer(RecoveryLaser);
			RemainingShots.Value++;
			_isRecovering = false;
		}

		private void ChangedTimeToRecovery(float recoveryTime)
		{
			TimeToRecovery.Value = recoveryTime;
		}
	}
}