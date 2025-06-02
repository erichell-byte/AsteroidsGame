using System;
using System.Collections.Generic;
using Config;
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
        private GameConfiguration config;

        public EnemiesFactory(
            Transform poolParent,
            GameConfiguration config)
        {
            this.config = config;
            asteroidPoolFacade = new EnemyPoolFacade(config.asteroidPrefab, poolParent);
            asteroidSmallPoolFacade = new EnemyPoolFacade(config.asteroidSmallPrefab, poolParent);
            ufoPoolFacade = new EnemyPoolFacade(config.ufoPrefab, poolParent);

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

        public Enemy CreateEnemy(EnemyType enemyType)
        {
            switch (enemyType)
            {
                case EnemyType.AsteroidSmall:
                {
                    var asteroidSmall = (AsteroidEnemy)asteroidSmallPoolFacade.Get();
                    asteroidSmall.Initialize(GetEnemyConfig(EnemyType.AsteroidSmall));
                    return asteroidSmall;
                }
                case EnemyType.Asteroid:
                {
                    var asteroid = (AsteroidEnemy)asteroidPoolFacade.Get();
                    asteroid.Initialize(GetEnemyConfig(EnemyType.Asteroid));
                    return asteroid;
                }
                case EnemyType.UFO:
                {
                    var ufo = (UFOEnemy)ufoPoolFacade.Get();
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