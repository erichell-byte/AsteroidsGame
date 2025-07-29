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
        private GameConfiguration _config;
        private TimersController _timersController;
        private MoveComponent _moveComponent;
        
        private readonly List<Enemy> _activeEnemies = new();
        private readonly List<Vector3> _spawnPoints = new();
        
        private Transform _poolParent;
        private EnemiesFactory _enemiesFactory;
        private IAssetLoader<Enemy> _loader;

        public Action<EnemyType> OnEnemyDeath;
            
        [Inject]
        private void Construct(
            TimersController timersController,
            GameConfiguration config,
            Transform poolParent,
            MoveComponent moveComponent,
            GameCycle gameCycle,
            IAssetLoader<Enemy> loader)
        {
            this._config = config;
            this._timersController = timersController;
            this._poolParent = poolParent;
            this._moveComponent = moveComponent;
            this._loader = loader;
            gameCycle.AddListener(this);
        }

        public void OnStartGame()
        {
            CreateSpawnPoints();
            _timersController.SubscribeToSpawnEnemies(SpawnAsteroid, SpawnUFO);

            _enemiesFactory = new EnemiesFactory(
                _poolParent,
                _config,
                _loader);
        }

        private void CreateSpawnPoints()
        {
            _spawnPoints.Clear();

            var camera = Camera.main;
            float height = camera!.orthographicSize;
            float width = height * camera.aspect;

            _spawnPoints.Add(new Vector3(-width - 2, 0, 0));
            _spawnPoints.Add(new Vector3(width + 2, 0, 0));
            _spawnPoints.Add(new Vector3(0, height + 2, 0));
            _spawnPoints.Add(new Vector3(0, -height - 2, 0));
        }

        private async void SpawnAsteroid()
        {
            var asteroid = await _enemiesFactory.CreateEnemy(EnemyType.Asteroid) as AsteroidEnemy;
            PrepareEnemy(asteroid);

            _timersController.PlaySpawnAsteroid();
        }

        private async void SpawnUFO()
        {
            var ufo = await _enemiesFactory.CreateEnemy(EnemyType.UFO) as UFOEnemy;
            ufo.InitMovement(_moveComponent.transform);
            PrepareEnemy(ufo);

            _timersController.PlaySpawnUFO();
        }

        private void PrepareEnemy(Enemy enemy)
        {
            enemy.transform.position = _spawnPoints[Random.Range(0, _spawnPoints.Count)];

            _activeEnemies.Add(enemy);
            enemy.OnDeath += OnDeathEnemy;
        }

        private async void SpawnSmallAsteroids(Vector3 position)
        {
            var firstAsteroid = await _enemiesFactory.CreateEnemy(EnemyType.AsteroidSmall) as AsteroidEnemy;
            var secondAsteroid = await _enemiesFactory.CreateEnemy(EnemyType.AsteroidSmall) as AsteroidEnemy;
            
            firstAsteroid.OnDeath += OnDeathEnemy;
            secondAsteroid.OnDeath += OnDeathEnemy;

            firstAsteroid.transform.position = secondAsteroid.transform.position = position;

            _activeEnemies.Add(firstAsteroid);
            _activeEnemies.Add(secondAsteroid);
        }
        
        private void OnDeathEnemy(Enemy enemy)
        {
            _activeEnemies.Remove(enemy);
            enemy.OnDeath -= OnDeathEnemy;
            
            OnEnemyDeath?.Invoke(enemy.GetEnemyType());
            
            if (enemy.GetEnemyType() == EnemyType.Asteroid)
            {
                SpawnSmallAsteroids(enemy.transform.position);
            }

            _enemiesFactory.ReturnEnemy(enemy);
        }

        public void OnFinishGame()
        {
            foreach (var enemy in _activeEnemies)
            {
                enemy.OnDeath -= OnDeathEnemy;
                Destroy(enemy.gameObject);
            }

            _timersController.UnsubscribeFromSpawnEnemies(SpawnAsteroid, SpawnUFO);

            _activeEnemies.Clear();
            _enemiesFactory?.Clear();
        }

        public void OnPauseGame()
        {
            _timersController.PauseEnemyTimers();
            for (int i = 0; i < _activeEnemies.Count; i++)
            {
                _activeEnemies[i].SetActive(false);
            }
        }

        public void OnResumeGame()
        {
            _timersController.ResumeEnemyTimers();
            for (int i = 0; i < _activeEnemies.Count; i++)
            {
                _activeEnemies[i].SetActive(true);
            }
        }
    }
}