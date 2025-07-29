using System;
using Components;
using Enemies;
using Systems;
using Zenject;

namespace Analytics
{
    public class AnalyticsMediator : IGameStartListener, IGameFinishListener, IDisposable
    {
        private IAnalyticsHandler _analyticsHandler;
        private AttackComponent _attackComponent;
        private EnemiesManager _enemiesManager;

        private int _laserShotCount;
        private int _mainShotCount;
        private int _asteroidsDestroyedCount;
        private int _ufoDestroyedCount;

        [Inject]
        private void Construct(
            IAnalyticsHandler analyticsHandler,
            AttackComponent attackComponent,
            EnemiesManager enemiesManager,
            GameCycle gameCycle)
        {
            this._analyticsHandler = analyticsHandler;
            this._attackComponent = attackComponent;
            this._enemiesManager = enemiesManager;

            this._attackComponent.LaserWeapon.OnLaserShot += OnLaserShot;
            this._attackComponent.MainWeapon.OnShot += IncreaseMainShotCount;
            this._enemiesManager.OnEnemyDeath += OnEnemyDeath;

            gameCycle.AddListener(this);
        }

        private void OnEnemyDeath(EnemyType enemyType)
        {
            if (enemyType == EnemyType.Asteroid || enemyType == EnemyType.AsteroidSmall)
            {
                _asteroidsDestroyedCount++;
            }
            else if (enemyType == EnemyType.UFO)
            {
                _ufoDestroyedCount++;
            }
        }

        public void OnStartGame()
        {
            SendStartGame();
        }

        public void OnFinishGame()
        {
            SendFinishGame();
        }

        private void SendStartGame()
        {
            _analyticsHandler.StartGame();
        }

        private void SendFinishGame()
        {
            _analyticsHandler.FinishGame(_mainShotCount, _laserShotCount, _asteroidsDestroyedCount, _ufoDestroyedCount);
            ResetCounters();
        }

        private void OnLaserShot()
        {
            _analyticsHandler.LaserShotFired();
            _laserShotCount++;
        }

        private void IncreaseMainShotCount()
        {
            _mainShotCount++;
        }

        private void ResetCounters()
        {
            _laserShotCount = 0;
            _mainShotCount = 0;
            _asteroidsDestroyedCount = 0;
            _ufoDestroyedCount = 0;
        }

        public void Dispose()
        {
            _attackComponent.LaserWeapon.OnLaserShot -= OnLaserShot;
            _attackComponent.MainWeapon.OnShot -= IncreaseMainShotCount;
            _enemiesManager.OnEnemyDeath -= OnEnemyDeath;
        }
    }
}