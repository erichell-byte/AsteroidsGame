using System;
using Config;
using GameSystem;
using Zenject;

namespace Utils
{
    public class TimersController
    {
        private Timer.Factory timersFactory;
        
        private Timer asteroidSpawnTimer;
        private Timer ufoSpawnTimer;
        private Timer laserRecoveryTimer;
        private Timer laserDurationTimer;
        private Timer shotTimer;

        private GameConfigurationSO config;
        
        [Inject]
        private void Construct(
            Timer.Factory timersFactory,
            GameConfigurationSO config)
        {
            this.timersFactory = timersFactory;
            this.config = config;
        }

        #region SpawnEnemies
        
        public void SubscribeToSpawnEnemies(Action spawnAsteroidAction, Action spawnUFOAction)
        {
            asteroidSpawnTimer = timersFactory.Create();
            asteroidSpawnTimer.Init(config.asteroidSpawnFrequency);
            ufoSpawnTimer = timersFactory.Create();
            ufoSpawnTimer.Init(config.ufoSpawnFrequency);

            asteroidSpawnTimer.Play();
            asteroidSpawnTimer.TimerIsExpired += spawnAsteroidAction;

            ufoSpawnTimer.Play();
            ufoSpawnTimer.TimerIsExpired += spawnUFOAction;
        }
        
        public void UnsubscribeFromSpawnEnemies(Action spawnAsteroidAction, Action spawnUFOAction)
        {
            if (asteroidSpawnTimer != null)
                asteroidSpawnTimer.TimerIsExpired -= spawnAsteroidAction;
            if (ufoSpawnTimer != null)
                ufoSpawnTimer.TimerIsExpired -= spawnUFOAction;
        }
        
        public void PauseEnemyTimers()
        {
            if (asteroidSpawnTimer != null)
                asteroidSpawnTimer.Pause();
            if (ufoSpawnTimer != null)
                ufoSpawnTimer.Pause();
        }
        
        public void ResumeEnemyTimers()
        {
            if (asteroidSpawnTimer != null)
                asteroidSpawnTimer.Resume();
            if (ufoSpawnTimer != null)
                ufoSpawnTimer.Resume();
        }

        public void PlaySpawnAsteroid()
        {
            asteroidSpawnTimer.Play();
        }
        
        public void PlaySpawnUFO()
        {
            ufoSpawnTimer.Play();
        }
        #endregion
        
        #region LaserWeapon
        
        public void InitLaserTimers(
            Action<float> changedTimeToRecoveryAction)
        {
            laserRecoveryTimer = timersFactory.Create();
            laserRecoveryTimer.Init(config.timeToRecoveryLaser);
            laserDurationTimer = timersFactory.Create();
            laserDurationTimer.Init(config.timeToDurationLaser);
            laserRecoveryTimer.RemainingTimeChanged += changedTimeToRecoveryAction;
        }

        public void PlayAndSubscribeDurationTimer(Action turnOffLaserAction)
        {
            laserDurationTimer.TimerIsExpired += turnOffLaserAction;
            laserDurationTimer.Play();
        }
        
        public void PlayAndSubscribeRecoveryTimer(Action recoveryLaserAction)
        {
            laserRecoveryTimer.TimerIsExpired += recoveryLaserAction;
            laserRecoveryTimer.Play();
        }
        
        public void UnsubscribeFromDurationTimer(Action turnOffLaserAction)
        {
            if (laserDurationTimer != null)
                laserDurationTimer.TimerIsExpired -= turnOffLaserAction;
        }
        
        public void UnsubscribeFromRecoveryTimer(Action recoveryLaserAction)
        {
            if (laserRecoveryTimer != null)
                laserRecoveryTimer.TimerIsExpired -= recoveryLaserAction;
        }
        
        public bool RecoveryTimerIsPlaying()
        {
            return laserRecoveryTimer != null && laserRecoveryTimer.IsPlaying();
        }
        
        public void UnsubscribeAllLaserTimers(Action<float> changedTimeToRecoveryAction, Action turnOffLaserAction, Action recoveryLaserAction)
        {
            if (laserRecoveryTimer != null)
                laserRecoveryTimer.RemainingTimeChanged -= changedTimeToRecoveryAction;
            if (laserDurationTimer != null)
                laserDurationTimer.TimerIsExpired -= turnOffLaserAction;
            if (laserRecoveryTimer != null)
                laserRecoveryTimer.TimerIsExpired -= recoveryLaserAction;
        }
        
        #endregion
        
        #region MainWeapon
        
        public void InitMainWeaponTimer()
        {
            shotTimer = timersFactory.Create();
            shotTimer.Init(config.shotFrequency);
        }
        
        public void PlayMainWeaponTimer()
        {
            shotTimer.Play();
        }
        
        public bool MainWeaponTimerIsPlaying()
        {
            return shotTimer != null && shotTimer.IsPlaying();
        }
        #endregion
    }
}