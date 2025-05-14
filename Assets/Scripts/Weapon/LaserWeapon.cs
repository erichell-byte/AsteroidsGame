using System;
using Enemies;
using GameSystem;
using UnityEngine;

namespace Weapon
{
    public class LaserWeapon : BaseWeapon
    {
        private readonly GameObject laser;
        private readonly LayerMask layerMask;
        private readonly Timer recoveryTimer;
        private readonly Timer durationTimer;
        private readonly int maxShots;

        private int remainingShots;
        private bool isActive;
        private bool isRecovering;

        public event Action<int> OnLaserCountChanged;
        public event Action<float> OnTimeToRecoveryChanged;

        public LaserWeapon(Transform shotPoint, GameObject laser, LayerMask layerMask, float recoveryTime,
            float durationTime, int maxShots) : base(shotPoint)
        {
            this.laser = laser;
            this.layerMask = layerMask;
            recoveryTimer = new Timer(recoveryTime);
            durationTimer = new Timer(durationTime);
            this.maxShots = maxShots;
            remainingShots = maxShots;
            recoveryTimer.RemainingTimeChanged += ChangedTimeToRecovery;
        }

        public override void Attack()
        {
            if (remainingShots <= 0 || isActive) return;

            remainingShots--;
            OnLaserCountChanged?.Invoke(remainingShots);
            isActive = true;
            laser.SetActive(true);
            durationTimer.Play();
            durationTimer.TimerIsExpired += TurnOffLaser;
        }

        public void Update()
        {
            HandleRecovery();
            HandleLaserDamage();
        }

        private void HandleRecovery()
        {
            if (!recoveryTimer.IsPlaying() && remainingShots < maxShots && !isRecovering)
            {
                isRecovering = true;
                recoveryTimer.Play();
                recoveryTimer.TimerIsExpired += RecoveryLaser;
            }
        }

        private void HandleLaserDamage()
        {
            if (isActive == false) return;

            var hit = Physics2D.Raycast(ShotPoint.position, ShotPoint.up, float.PositiveInfinity, layerMask);

            if (hit.collider == null) return;
            if (hit.collider.TryGetComponent(out Enemy enemy)) enemy.Die();
        }

        public void TurnOffLaser()
        {
            durationTimer.TimerIsExpired -= TurnOffLaser;
            isActive = false;
            laser.SetActive(false);
        }

        private void RecoveryLaser()
        {
            recoveryTimer.TimerIsExpired -= RecoveryLaser;
            remainingShots++;
            OnLaserCountChanged?.Invoke(remainingShots);
            isRecovering = false;
        }

        private void ChangedTimeToRecovery(float recoveryTime)
        {
            OnTimeToRecoveryChanged?.Invoke(recoveryTime);
        }

        ~LaserWeapon()
        {
            recoveryTimer.RemainingTimeChanged -= ChangedTimeToRecovery;
        }
    }
}