using System;
using Enemies;
using UniRx;
using UnityEngine;

namespace Systems
{
	public interface IGameEvents
	{
		IObservable<Unit> OnSpaceshipCollidedWithEnemy { get; }
		IObservable<Vector3> OnSpaceshipShot { get; }
		IObservable<(Vector3, EnemyType)> OnEnemyKilled { get; }
		void NotifySpaceshipCollidedWithEnemy();
		void NotifySpaceshipShot(Vector3 position);
		void NotifyEnemyKilled(Vector3 position, EnemyType enemyType);
	}
}