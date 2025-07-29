using System;
using System.Collections.Generic;
using AssetsLoader;
using Config;
using Cysharp.Threading.Tasks;
using Pools;
using UnityEngine;

namespace Enemies
{
    public enum EnemyType
    {
        Asteroid,
        AsteroidSmall,
        UFO
    }

    public class EnemiesFactory
    {
        private readonly EnemyPoolFacade _asteroidPoolFacade;
        private readonly EnemyPoolFacade _asteroidSmallPoolFacade;
        private readonly EnemyPoolFacade _ufoPoolFacade;
        private readonly GameConfiguration _config;
        
        private Dictionary<EnemyType, EnemyConfig> _enemyConfigMap;
        
        public EnemiesFactory(
            Transform poolParent,
            GameConfiguration config,
            IAssetLoader<Enemy> loader)
        {
            this._config = config;
            _asteroidPoolFacade = new EnemyPoolFacade(loader,config.AsteroidId, poolParent);
            _asteroidSmallPoolFacade = new EnemyPoolFacade(loader, config.AsteroidSmallId, poolParent);
            _ufoPoolFacade = new EnemyPoolFacade(loader, config.UfoId, poolParent);

            BuildEnemyConfigMap();
        }
        
        private void BuildEnemyConfigMap()
        {
            _enemyConfigMap = new Dictionary<EnemyType, EnemyConfig>();
            foreach (var cfg in _config.EnemiesConfigs)
            {
                if (!_enemyConfigMap.ContainsKey(cfg.Type))
                    _enemyConfigMap.Add(cfg.Type, cfg);
            }
        }
        
        private EnemyConfig GetEnemyConfig(EnemyType enemyType)
        {
            if (_enemyConfigMap != null && _enemyConfigMap.TryGetValue(enemyType, out var foundConfig))
                return foundConfig;
            throw new ArgumentException($"Unknown enemy type: {enemyType}");
        }

        public async UniTask<Enemy> CreateEnemy(EnemyType enemyType)
        {
            switch (enemyType)
            {
                case EnemyType.AsteroidSmall:
                {
                    var enemy = await _asteroidSmallPoolFacade.GetAsync();
                    var asteroidSmall = (AsteroidEnemy)enemy;
                    asteroidSmall.Initialize(GetEnemyConfig(EnemyType.AsteroidSmall));
                    return asteroidSmall;
                }
                case EnemyType.Asteroid:
                {
                    var enemy = await _asteroidPoolFacade.GetAsync();
                    var asteroid = (AsteroidEnemy)enemy;
                    asteroid.Initialize(GetEnemyConfig(EnemyType.Asteroid));
                    return asteroid;
                }
                case EnemyType.UFO:
                {
                    var enemy = await _ufoPoolFacade.GetAsync();
                    var ufo = (UFOEnemy)enemy;
                    ufo.Initialize(GetEnemyConfig(EnemyType.UFO));
                    return ufo;
                }
            }

            throw new Exception("Enemy type not recognized");
        }

        public void ReturnEnemy(Enemy enemy)
        {
            switch (enemy.GetEnemyType())
            {
                case EnemyType.AsteroidSmall:
                    _asteroidSmallPoolFacade.Release(enemy);
                    break;
                case EnemyType.Asteroid:
                    _asteroidPoolFacade.Release(enemy);
                    break;
                case EnemyType.UFO:
                    _ufoPoolFacade.Release(enemy);
                    break;
            }
        }

        public void Clear()
        {
            _asteroidPoolFacade.Clear();
            _ufoPoolFacade.Clear();
        }
    }
}