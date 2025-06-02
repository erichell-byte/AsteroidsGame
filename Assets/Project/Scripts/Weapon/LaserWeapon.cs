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
        private TimersController timersController;

        private GameObject laser;
        private LayerMask layerMask;
        private int maxShots;
        private bool isActive;
        private bool isRecovering;
        private bool disposed;

        public ReactiveProperty<int> RemainingShots { get; } = new();
        public ReactiveProperty<float> TimeToRecovery { get; } = new();
        [Inject]
        private void Construct(
            TimersController timersController)
        {
            this.timersController = timersController;
        }

        public void Initialize(
            Transform shotPoint,
            GameObject laser,
            LayerMask layerMask,
            int maxShots)
        {
            this.shotPoint = shotPoint;
            this.laser = laser;
            this.layerMask = layerMask;
            this.maxShots = maxShots;
            RemainingShots.Value = maxShots;
            TimeToRecovery.Value = 0f;
            timersController.InitLaserTimers(ChangedTimeToRecovery);
        }

        public override void Attack()
        {
            if (RemainingShots.Value <= 0 || isActive) return;

            RemainingShots.Value--;
            isActive = true;
            laser.SetActive(true);
            timersController.PlayAndSubscribeDurationTimer(TurnOffLaser);
        }

        public void Tick()
        {
            HandleRecovery();
            HandleLaserDamage();
        }

        private void HandleRecovery()
        {
            if (timersController.RecoveryTimerIsPlaying() == false && RemainingShots.Value < maxShots && !isRecovering)
            {
                isRecovering = true;
                timersController.PlayAndSubscribeRecoveryTimer(RecoveryLaser);
            }
        }

        private void HandleLaserDamage()
        {
            if (isActive == false) return;

            var hit = Physics2D.Raycast(shotPoint.position, shotPoint.up, float.PositiveInfinity, layerMask);

            if (hit.collider == null) return;
            if (hit.collider.TryGetComponent(out Enemy enemy)) enemy.Die();
        }

        public void TurnOffLaser()
        {
            timersController.UnsubscribeFromDurationTimer(TurnOffLaser);
            isActive = false;
            laser.SetActive(false);
        }

        private void RecoveryLaser()
        {
            timersController.UnsubscribeFromRecoveryTimer(RecoveryLaser);
            RemainingShots.Value++;
            isRecovering = false;
        }

        private void ChangedTimeToRecovery(float recoveryTime)
        {
            TimeToRecovery.Value = recoveryTime;
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            timersController.UnsubscribeAllLaserTimers(ChangedTimeToRecovery, TurnOffLaser, RecoveryLaser);
        }
    }
}