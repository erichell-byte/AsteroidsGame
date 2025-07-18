using System;
using System.Collections.Generic;
using AssetsLoader;
using Components;
using Config;
using Systems;
using UnityEngine;
using Utils;
using Zenject;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemiesManager : MonoBehaviour,
        IGameStartListener,
        IGameFinishListener,
        IGamePauseListener,
        IGameResumeListener
    {
        private GameConfigurationSO config;
        private TimersController timersController;
        private MoveComponent moveComponent;
        
        private Transform poolParent;
        private List<Vector3> spawnPoints = new();
        private EnemiesFactory enemiesFactory;
        private List<Enemy> activeEnemies = new();
        private IAssetLoader<Enemy> loader;

        public Action<EnemyType> OnEnemyDeath;
            
        [Inject]
        private void Construct(
            TimersController timersController,
            GameConfigurationSO config,
            Transform poolParent,
            MoveComponent moveComponent,
            GameCycle gameCycle,
            IAssetLoader<Enemy> loader)
        {
            this.config = config;
            this.timersController = timersController;
            this.poolParent = poolParent;
            this.moveComponent = moveComponent;
            this.loader = loader;
            gameCycle.AddListener(this);
        }

        public void OnStartGame()
        {
            CreateSpawnPoints();
            timersController.SubscribeToSpawnEnemies(SpawnAsteroid, SpawnUFO);

            enemiesFactory = new EnemiesFactory(
                poolParent,
                config,
                loader);
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

        private async void SpawnAsteroid()
        {
            var asteroid = await enemiesFactory.CreateEnemy(EnemyType.Asteroid) as AsteroidEnemy;
            PrepareEnemy(asteroid);

            timersController.PlaySpawnAsteroid();
        }

        private async void SpawnUFO()
        {
            var ufo = await enemiesFactory.CreateEnemy(EnemyType.UFO) as UFOEnemy;
            ufo.InitMovement(moveComponent.transform);
            PrepareEnemy(ufo);

            timersController.PlaySpawnUFO();
        }

        private void PrepareEnemy(Enemy enemy)
        {
            enemy.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)];

            activeEnemies.Add(enemy);
            enemy.OnDeath += OnDeathEnemy;
        }

        private async void SpawnSmallAsteroids(Vector3 position)
        {
            var firstAsteroid = await enemiesFactory.CreateEnemy(EnemyType.AsteroidSmall) as AsteroidEnemy;
            var secondAsteroid = await enemiesFactory.CreateEnemy(EnemyType.AsteroidSmall) as AsteroidEnemy;
            
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
            
            OnEnemyDeath?.Invoke(enemy.GetEnemyType());
            
            if (enemy.GetEnemyType() == EnemyType.Asteroid)
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

        public void OnPauseGame()
        {
            timersController.PauseEnemyTimers();
            for (int i = 0; i < activeEnemies.Count; i++)
            {
                activeEnemies[i].SetActive(false);
            }
        }

        public void OnResumeGame()
        {
            timersController.ResumeEnemyTimers();
            for (int i = 0; i < activeEnemies.Count; i++)
            {
                activeEnemies[i].SetActive(true);
            }
        }
    }
}