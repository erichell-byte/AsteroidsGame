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
		private readonly List<Enemy> _activeEnemies = new();
		private readonly List<Vector3> _spawnPoints = new();
		private GameConfiguration _config;
		private EnemiesFactory _enemiesFactory;
		private IGameEvents _gameEvents;
		private IAssetLoader<Enemy> _loader;
		private MoveComponent _moveComponent;

		private Transform _poolParent;
		private IAssetsPreloader _preloader;
		private TimersController _timersController;

		public Action<EnemyType> OnEnemyDeath;

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

			_preloader.ReleaseAll();
		}

		public void OnPauseGame()
		{
			_timersController.PauseEnemyTimers();
			for (var i = 0; i < _activeEnemies.Count; i++) _activeEnemies[i].SetActive(false);
		}

		public void OnResumeGame()
		{
			_timersController.ResumeEnemyTimers();
			for (var i = 0; i < _activeEnemies.Count; i++) _activeEnemies[i].SetActive(true);
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

		[Inject]
		private void Construct(
			TimersController timersController,
			GameConfiguration config,
			Transform poolParent,
			MoveComponent moveComponent,
			GameCycle gameCycle,
			IAssetLoader<Enemy> loader,
			IAssetsPreloader preloader,
			IGameEvents gameEvents)
		{
			_config = config;
			_timersController = timersController;
			_poolParent = poolParent;
			_moveComponent = moveComponent;
			_loader = loader;
			_preloader = preloader;
			_gameEvents = gameEvents;
			gameCycle.AddListener(this);
		}

		private void CreateSpawnPoints()
		{
			_spawnPoints.Clear();

			var camera = Camera.main;
			var height = camera!.orthographicSize;
			var width = height * camera.aspect;

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
			_gameEvents.NotifyEnemyKilled(enemy.transform.position, enemy.GetEnemyType());
			_activeEnemies.Remove(enemy);
			enemy.OnDeath -= OnDeathEnemy;

			OnEnemyDeath?.Invoke(enemy.GetEnemyType());

			if (enemy.GetEnemyType() == EnemyType.Asteroid) SpawnSmallAsteroids(enemy.transform.position);

			_enemiesFactory.ReturnEnemy(enemy);
		}
	}
}