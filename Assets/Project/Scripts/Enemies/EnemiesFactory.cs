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
        private EnemyPoolFacade asteroidPoolFacade;
        private EnemyPoolFacade asteroidSmallPoolFacade;
        private EnemyPoolFacade ufoPoolFacade;
        private Dictionary<EnemyType, EnemyConfig> enemyConfigMap;
        private GameConfigurationSO config;

        public EnemiesFactory(
            Transform poolParent,
            GameConfigurationSO config,
            IAssetLoader<Enemy> loader)
        {
            this.config = config;
            asteroidPoolFacade = new EnemyPoolFacade(loader,config.asteroidId, poolParent);
            asteroidSmallPoolFacade = new EnemyPoolFacade(loader, config.asteroidSmallId, poolParent);
            ufoPoolFacade = new EnemyPoolFacade(loader, config.ufoId, poolParent);

            BuildEnemyConfigMap();
        }
        
        private void BuildEnemyConfigMap()
        {
            enemyConfigMap = new Dictionary<EnemyType, EnemyConfig>();
            foreach (var cfg in config.enemiesConfigs)
            {
                if (!enemyConfigMap.ContainsKey(cfg.type))
                    enemyConfigMap.Add(cfg.type, cfg);
            }
        }
        
        private EnemyConfig GetEnemyConfig(EnemyType enemyType)
        {
            if (enemyConfigMap != null && enemyConfigMap.TryGetValue(enemyType, out var foundConfig))
                return foundConfig;
            throw new ArgumentException($"Unknown enemy type: {enemyType}");
        }

        public async UniTask<Enemy> CreateEnemy(EnemyType enemyType)
        {
            switch (enemyType)
            {
                case EnemyType.AsteroidSmall:
                {
                    var enemy = await asteroidSmallPoolFacade.GetAsync();
                    var asteroidSmall = (AsteroidEnemy)enemy;
                    asteroidSmall.Initialize(GetEnemyConfig(EnemyType.AsteroidSmall));
                    return asteroidSmall;
                }
                case EnemyType.Asteroid:
                {
                    var enemy = await asteroidPoolFacade.GetAsync();
                    var asteroid = (AsteroidEnemy)enemy;
                    asteroid.Initialize(GetEnemyConfig(EnemyType.Asteroid));
                    return asteroid;
                }
                case EnemyType.UFO:
                {
                    var enemy = await ufoPoolFacade.GetAsync();
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
                    asteroidSmallPoolFacade.Release(enemy);
                    break;
                case EnemyType.Asteroid:
                    asteroidPoolFacade.Release(enemy);
                    break;
                case EnemyType.UFO:
                    ufoPoolFacade.Release(enemy);
                    break;
            }
        }

        public void Clear()
        {
            asteroidPoolFacade.Clear();
            ufoPoolFacade.Clear();
        }
    }
}