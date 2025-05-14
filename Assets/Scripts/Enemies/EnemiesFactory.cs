using System;
using Pools;
using UnityEngine;

namespace Enemies
{
    public enum EnemyType
    {
        AsteroidBig,
        AsteroidSmall,
        UFO
    }

    public class EnemiesFactory
    {
        private EnemyPoolFacade asteroidBigPoolFacade;
        private EnemyPoolFacade asteroidSmallPoolFacade;
        private EnemyPoolFacade ufoPoolFacade;

        public EnemiesFactory(
            AsteroidBigEnemy asteroidBigPrefab,
            AsteroidSmallEnemy asteroidSmallPrefab,
            UFOEnemy ufoPrefab,
            Transform poolParent)
        {
            asteroidBigPoolFacade = new EnemyPoolFacade(asteroidBigPrefab, poolParent);
            asteroidSmallPoolFacade = new EnemyPoolFacade(asteroidSmallPrefab, poolParent);
            ufoPoolFacade = new EnemyPoolFacade(ufoPrefab, poolParent);
        }

        public Enemy CreateEnemy(EnemyType enemyType)
        {
            switch (enemyType)
            {
                case EnemyType.AsteroidBig:
                    return (AsteroidBigEnemy)asteroidBigPoolFacade.Pool.Get();
                case EnemyType.AsteroidSmall:
                    return (AsteroidSmallEnemy)asteroidSmallPoolFacade.Pool.Get();
                case EnemyType.UFO:
                    return (UFOEnemy)ufoPoolFacade.Pool.Get();
            }

            throw new Exception("Enemy type not recognized");
        }

        public void ReturnEnemy(EnemyType enemyType, Enemy enemy)
        {
            switch (enemyType)
            {
                case EnemyType.AsteroidSmall:
                    asteroidSmallPoolFacade.Pool.Release(enemy);
                    break;
                case EnemyType.AsteroidBig:
                    asteroidBigPoolFacade.Pool.Release(enemy);
                    break;
                case EnemyType.UFO:
                    ufoPoolFacade.Pool.Release(enemy);
                    break;
            }
        }

        public void Clear()
        {
            asteroidBigPoolFacade.Pool.Clear();
            asteroidSmallPoolFacade.Pool.Clear();
            ufoPoolFacade.Pool.Clear();
        }
    }
}