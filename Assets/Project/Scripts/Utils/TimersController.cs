using System;
using Config;
using Zenject;

namespace Utils
{
	public class TimersController
	{
		private Timer _asteroidSpawnTimer;
		private Timer _laserDurationTimer;
		private Timer _laserRecoveryTimer;

		private RemoteConfig _remoteConfig;
		private Timer _shotTimer;
		private Timer.Factory _timersFactory;
		private Timer _ufoSpawnTimer;

		[Inject]
		private void Construct(
			Timer.Factory timersFactory,
			IConfigProvider configProvider)
		{
			this._timersFactory = timersFactory;

			_remoteConfig = configProvider.GetRemoteConfig();
		}

		#region SpawnEnemies

		public void SubscribeToSpawnEnemies(Action spawnAsteroidAction, Action spawnUFOAction)
		{
			_asteroidSpawnTimer = _timersFactory.Create();
			_asteroidSpawnTimer.Init(_remoteConfig.AsteroidSpawnFrequency);
			_ufoSpawnTimer = _timersFactory.Create();
			_ufoSpawnTimer.Init(_remoteConfig.UfoSpawnFrequency);

			_asteroidSpawnTimer.Play();
			_asteroidSpawnTimer.TimerIsExpired += spawnAsteroidAction;

			_ufoSpawnTimer.Play();
			_ufoSpawnTimer.TimerIsExpired += spawnUFOAction;
		}

		public void UnsubscribeFromSpawnEnemies(Action spawnAsteroidAction, Action spawnUFOAction)
		{
			if (_asteroidSpawnTimer != null)
				_asteroidSpawnTimer.TimerIsExpired -= spawnAsteroidAction;
			if (_ufoSpawnTimer != null)
				_ufoSpawnTimer.TimerIsExpired -= spawnUFOAction;
		}

		public void PauseEnemyTimers()
		{
			if (_asteroidSpawnTimer != null)
				_asteroidSpawnTimer.Pause();
			if (_ufoSpawnTimer != null)
				_ufoSpawnTimer.Pause();
		}

		public void ResumeEnemyTimers()
		{
			if (_asteroidSpawnTimer != null)
				_asteroidSpawnTimer.Resume();
			if (_ufoSpawnTimer != null)
				_ufoSpawnTimer.Resume();
		}

		public void PlaySpawnAsteroid()
		{
			_asteroidSpawnTimer.Play();
		}

		public void PlaySpawnUFO()
		{
			_ufoSpawnTimer.Play();
		}

		#endregion

		#region LaserWeapon

		public void InitLaserTimers(
			Action<float> changedTimeToRecoveryAction)
		{
			_laserRecoveryTimer = _timersFactory.Create();
			_laserRecoveryTimer.Init(_remoteConfig.TimeToRecoveryLaser);
			_laserDurationTimer = _timersFactory.Create();
			_laserDurationTimer.Init(_remoteConfig.TimeToDurationLaser);
			_laserRecoveryTimer.RemainingTimeChanged += changedTimeToRecoveryAction;
		}

		public void PlayAndSubscribeDurationTimer(Action turnOffLaserAction)
		{
			_laserDurationTimer.TimerIsExpired += turnOffLaserAction;
			_laserDurationTimer.Play();
		}

		public void PlayAndSubscribeRecoveryTimer(Action recoveryLaserAction)
		{
			_laserRecoveryTimer.TimerIsExpired += recoveryLaserAction;
			_laserRecoveryTimer.Play();
		}

		public void UnsubscribeFromDurationTimer(Action turnOffLaserAction)
		{
			if (_laserDurationTimer != null)
				_laserDurationTimer.TimerIsExpired -= turnOffLaserAction;
		}

		public void UnsubscribeFromRecoveryTimer(Action recoveryLaserAction)
		{
			if (_laserRecoveryTimer != null)
				_laserRecoveryTimer.TimerIsExpired -= recoveryLaserAction;
		}

		public bool RecoveryTimerIsPlaying()
		{
			return _laserRecoveryTimer != null && _laserRecoveryTimer.IsPlaying();
		}

		public void UnsubscribeAllLaserTimers(Action<float> changedTimeToRecoveryAction, Action turnOffLaserAction,
			Action recoveryLaserAction)
		{
			if (_laserRecoveryTimer != null)
				_laserRecoveryTimer.RemainingTimeChanged -= changedTimeToRecoveryAction;
			if (_laserDurationTimer != null)
				_laserDurationTimer.TimerIsExpired -= turnOffLaserAction;
			if (_laserRecoveryTimer != null)
				_laserRecoveryTimer.TimerIsExpired -= recoveryLaserAction;
		}

		#endregion

		#region MainWeapon

		public void InitMainWeaponTimer()
		{
			_shotTimer = _timersFactory.Create();
			_shotTimer.Init(_remoteConfig.ShotFrequency);
		}

		public void PlayMainWeaponTimer()
		{
			_shotTimer.Play();
		}

		public bool MainWeaponTimerIsPlaying()
		{
			return _shotTimer != null && _shotTimer.IsPlaying();
		}

		#endregion
	}
}