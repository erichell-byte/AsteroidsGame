using System;
using Enemies;
using UniRx;
using UnityEngine;

namespace Systems
{
	public class GameEvents : IGameEvents
	{
		private readonly Subject<(Vector3, EnemyType)> _enemyKilled = new();
		private readonly Subject<Unit> _spaceshipCollidedWithEnemy = new();
		private readonly Subject<Vector3> _spaceshipShot = new();

		public IObservable<Unit> OnSpaceshipCollidedWithEnemy => _spaceshipCollidedWithEnemy;
		public IObservable<Vector3> OnSpaceshipShot => _spaceshipShot;
		public IObservable<(Vector3, EnemyType)> OnEnemyKilled => _enemyKilled;

		public void NotifySpaceshipCollidedWithEnemy()
		{
			_spaceshipCollidedWithEnemy.OnNext(Unit.Default);
		}

		public void NotifySpaceshipShot(Vector3 position)
		{
			_spaceshipShot.OnNext(position);
		}

		public void NotifyEnemyKilled(Vector3 position, EnemyType enemyType)
		{
			_enemyKilled.OnNext((position, enemyType));
		}
	}
}