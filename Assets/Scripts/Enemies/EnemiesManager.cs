using System.Collections.Generic;
using Character;
using Config;
using GameSystem;
using Systems;
using UnityEngine;

namespace Enemies
{
    public class EnemiesManager : MonoBehaviour, IGameStartListener, IGameFinishListener
    {
        [SerializeField] private GameConfiguration config;
        [SerializeField] private Transform poolParent;
        [SerializeField] private SpaceshipController spaceship;
        [SerializeField] private PointStorage pointStorage;

        private List<Vector3> spawnPoints = new();

        private EnemiesFactory enemiesFactory;
        private Timer asteroidSpawnTimer;
        private Timer ufoSpawnTimer;
        private List<Enemy> activeEnemies = new();

        public void OnStartGame()
        {
            CreateSpawnPoints();
            PrepareTimers();

            enemiesFactory = new EnemiesFactory(
                config.asteroidBigPrefab,
                config.asteroidSmallPrefab,
                config.ufoPrefab,
                poolParent);
        }

        private void PrepareTimers()
        {
            asteroidSpawnTimer = new Timer(config.asteroidSpawnFrequency);
            ufoSpawnTimer = new Timer(config.ufoSpawnFrequency);

            asteroidSpawnTimer.Play();
            asteroidSpawnTimer.TimerIsExpired += SpawnAsteroid;

            ufoSpawnTimer.Play();
            ufoSpawnTimer.TimerIsExpired += SpawnUFO;
        }

        private void CreateSpawnPoints()
        {
            spawnPoints.Clear();

            var camera = Camera.main;
            float height = camera!.orthographicSize;
            float width = height * camera.aspect;

            spawnPoints.Add(new Vector3(-width - 2, 0, 0));
            spawnPoints.Add(new Vector3(width + 2, 0, 0));
            spawnPoints.Add(new Vector3(0, height + 2, 0));
            spawnPoints.Add(new Vector3(0, -height - 2, 0));
        }

        private void SpawnAsteroid()
        {
            var asteroid = (AsteroidBigEnemy)enemiesFactory.CreateEnemy(EnemyType.AsteroidBig);
            PrepareEnemy(asteroid);

            asteroidSpawnTimer.Play();
        }

        private void SpawnUFO()
        {
            var ufo = (UFOEnemy)enemiesFactory.CreateEnemy(EnemyType.UFO);
            ufo.SetTarget(spaceship.transform);
            PrepareEnemy(ufo);

            ufoSpawnTimer.Play();
        }

        private void PrepareEnemy(Enemy enemy)
        {
            enemy.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)];

            activeEnemies.Add(enemy);
            enemy.Initialize(GetEnemyConfig(enemy));
            enemy.OnDeath += OnDeathEnemy;
        }

        private void SpawnSmallAsteroids(Vector3 position)
        {
            var firstAsteroid = (AsteroidSmallEnemy)enemiesFactory.CreateEnemy(EnemyType.AsteroidSmall);
            var secondAsteroid = (AsteroidSmallEnemy)enemiesFactory.CreateEnemy(EnemyType.AsteroidSmall);

            firstAsteroid.transform.position = position;
            secondAsteroid.transform.position = position;

            activeEnemies.Add(firstAsteroid);
            activeEnemies.Add(secondAsteroid);

            firstAsteroid.Initialize(GetEnemyConfig(firstAsteroid));
            secondAsteroid.Initialize(GetEnemyConfig(secondAsteroid));

            firstAsteroid.OnDeath += OnDeathEnemy;
            secondAsteroid.OnDeath += OnDeathEnemy;
        }

        private EnemyConfig GetEnemyConfig(Enemy enemy)
        {
            return enemy switch
            {
                AsteroidSmallEnemy => config.asteroidSmallConfig,
                AsteroidBigEnemy => config.asteroidBigConfig,
                UFOEnemy => config.ufoConfig,
                _ => throw new System.ArgumentException("Unknown enemy type")
            };
        }

        private void OnDeathEnemy(Enemy enemy)
        {
            pointStorage.AddPoints(enemy.GetPoints());
            activeEnemies.Remove(enemy);

            enemy.OnDeath -= OnDeathEnemy;

            if (enemy is AsteroidSmallEnemy)
            {
                enemiesFactory.ReturnEnemy(EnemyType.AsteroidSmall, enemy);
            }
            else if (enemy is AsteroidBigEnemy)
            {
                SpawnSmallAsteroids(enemy.transform.position);
                enemiesFactory.ReturnEnemy(EnemyType.AsteroidBig, enemy);
            }
            else if (enemy is UFOEnemy)
            {
                enemiesFactory.ReturnEnemy(EnemyType.UFO, enemy);
            }
        }

        public void OnFinishGame()
        {
            for (int i = 0; i < activeEnemies.Count; i++)
            {
                activeEnemies[i].OnDeath -= OnDeathEnemy;
                Destroy(activeEnemies[i].gameObject);
            }

            asteroidSpawnTimer.TimerIsExpired -= SpawnAsteroid;
            ufoSpawnTimer.TimerIsExpired -= SpawnUFO;

            activeEnemies.Clear();
            enemiesFactory.Clear();
        }
    }
}