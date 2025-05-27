using System;
using Enemies;
using GameSystem;
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
        

        private int remainingShots;
        private bool isActive;
        private bool isRecovering;
        private bool disposed;

        [Inject]
        private void Construct(
            TimersController timersController)
        {
            this.timersController = timersController;
        }

        public event Action<int> OnLaserCountChanged;
        public event Action<float> OnTimeToRecoveryChanged;

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
            remainingShots = maxShots;
            timersController.InitLaserTimers(ChangedTimeToRecovery);
        }

        public override void Attack()
        {
            if (remainingShots <= 0 || isActive) return;

            remainingShots--;
            OnLaserCountChanged?.Invoke(remainingShots);
            isActive = true;
            laser.SetActive(true);
            timersController.PlayAndSubscribeDurationTimer(TurnOffLaser);
        }

        public void Update()
        {
            HandleRecovery();
            HandleLaserDamage();
        }

        private void HandleRecovery()
        {
            if (timersController.RecoveryTimerIsPlaying() == false && remainingShots < maxShots && !isRecovering)
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
            remainingShots++;
            OnLaserCountChanged?.Invoke(remainingShots);
            isRecovering = false;
        }

        private void ChangedTimeToRecovery(float recoveryTime)
        {
            OnTimeToRecoveryChanged?.Invoke(recoveryTime);
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            timersController.UnsubscribeAllLaserTimers(ChangedTimeToRecovery, TurnOffLaser, RecoveryLaser);
        }
    }
}