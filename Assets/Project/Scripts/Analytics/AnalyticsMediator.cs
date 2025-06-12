using System;
using Components;
using Enemies;
using Systems;
using Zenject;

namespace Analytics
{
    public class AnalyticsMediator : IGameStartListener, IGameFinishListener, IDisposable
    {
        private IAnalyticsHandler analyticsHandler;
        private AttackComponent attackComponent;
        private EnemiesManager enemiesManager;

        private int laserShotCount;
        private int mainShotCount;
        private int asteroidsDestroyedCount;
        private int ufoDestroyedCount;

        [Inject]
        private void Construct(
            IAnalyticsHandler analyticsHandler,
            AttackComponent attackComponent,
            EnemiesManager enemiesManager,
            GameCycle gameCycle)
        {
            this.analyticsHandler = analyticsHandler;
            this.attackComponent = attackComponent;
            this.enemiesManager = enemiesManager;

            this.attackComponent.LaserWeapon.OnLaserShot += OnLaserShot;
            this.attackComponent.MainWeapon.OnShot += IncreaseMainShotCount;
            this.enemiesManager.OnEnemyDeath += OnEnemyDeath;

            gameCycle.AddListener(this);
        }

        private void OnEnemyDeath(EnemyType enemyType)
        {
            if (enemyType == EnemyType.Asteroid || enemyType == EnemyType.AsteroidSmall)
            {
                asteroidsDestroyedCount++;
            }
            else if (enemyType == EnemyType.UFO)
            {
                ufoDestroyedCount++;
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
            analyticsHandler.StartGame();
        }

        private void SendFinishGame()
        {
            analyticsHandler.FinishGame(mainShotCount, laserShotCount, asteroidsDestroyedCount, ufoDestroyedCount);
            ResetCounters();
        }

        private void OnLaserShot()
        {
            analyticsHandler.LaserShotFired();
            laserShotCount++;
        }

        private void IncreaseMainShotCount()
        {
            mainShotCount++;
        }

        private void ResetCounters()
        {
            laserShotCount = 0;
            mainShotCount = 0;
            asteroidsDestroyedCount = 0;
            ufoDestroyedCount = 0;
        }

        public void Dispose()
        {
            attackComponent.LaserWeapon.OnLaserShot -= OnLaserShot;
            attackComponent.MainWeapon.OnShot -= IncreaseMainShotCount;
            enemiesManager.OnEnemyDeath -= OnEnemyDeath;
        }
    }
}