using System.Collections.Generic;
using Character;
using Config;
using GameSystem;
using Systems;
using UnityEngine;
using Utils;
using Zenject;

namespace Enemies
{
    public class EnemiesManager : MonoBehaviour, IGameStartListener, IGameFinishListener
    {
        private GameConfiguration config;
        private TimersController timersController;
        
        [SerializeField] private Transform poolParent;
        
        private SpaceshipController spaceship;
        private List<Vector3> spawnPoints = new();
        private EnemiesFactory enemiesFactory;
        private List<Enemy> activeEnemies = new();
        

        [Inject]
        private void Construct(
            TimersController timersController,
            GameConfiguration config,
            SpaceshipController spaceship)
        {
            this.config = config;
            this.timersController = timersController;
            this.spaceship = spaceship;
        }

        public void OnStartGame()
        {
            CreateSpawnPoints();
            timersController.SubscribeToSpawnEnemies(SpawnAsteroid, SpawnUFO);

            enemiesFactory = new EnemiesFactory(
                poolParent,
                config);
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
            var asteroid = enemiesFactory.CreateEnemy(EnemyType.Asteroid) as AsteroidEnemy;
            PrepareEnemy(asteroid);

            timersController.PlaySpawnAsteroid();
        }

        private void SpawnUFO()
        {
            var ufo = enemiesFactory.CreateEnemy(EnemyType.UFO) as UFOEnemy;
            ufo.SetTarget(spaceship.transform);
            PrepareEnemy(ufo);

            timersController.PlaySpawnUFO();
        }

        private void PrepareEnemy(Enemy enemy)
        {
            enemy.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)];

            activeEnemies.Add(enemy);
            enemy.OnDeath += OnDeathEnemy;
        }

        private void SpawnSmallAsteroids(Vector3 position)
        {
            var firstAsteroid = enemiesFactory.CreateEnemy(EnemyType.AsteroidSmall) as AsteroidEnemy;
            var secondAsteroid = enemiesFactory.CreateEnemy(EnemyType.AsteroidSmall) as AsteroidEnemy;
            
            firstAsteroid.OnDeath += OnDeathEnemy;
            secondAsteroid.OnDeath += OnDeathEnemy;

            firstAsteroid.transform.position = secondAsteroid.transform.position = position;

            activeEnemies.Add(firstAsteroid);
            activeEnemies.Add(secondAsteroid);
        }
        
        private void OnDeathEnemy(Enemy enemy)
        {
            activeEnemies.Remove(enemy);
            enemy.OnDeath -= OnDeathEnemy;

            if (enemy is AsteroidEnemy asteroidEnemy && asteroidEnemy.IsShouldDestroyOnSmall())
            {
                SpawnSmallAsteroids(enemy.transform.position);
            }

            enemiesFactory.ReturnEnemy(enemy);
        }

        public void OnFinishGame()
        {
            foreach (var enemy in activeEnemies)
            {
                enemy.OnDeath -= OnDeathEnemy;
                Destroy(enemy.gameObject);
            }

            timersController.UnsubscribeFromSpawnEnemies(SpawnAsteroid, SpawnUFO);

            activeEnemies.Clear();
            enemiesFactory?.Clear();
        }
    }
}