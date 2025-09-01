using System;
using AssetsLoader;
using Config;
using Cysharp.Threading.Tasks;
using Enemies;
using Systems;
using UniRx;
using UnityEngine;

public class EffectsMediator : IDisposable
{
	private readonly EffectPoolFacade _asteroidPool;
	private readonly EffectPoolFacade _shotPool;
	private readonly EffectPoolFacade _ufoPool;

	private readonly CompositeDisposable _disposables = new();

	public EffectsMediator(
		IGameEvents events,
		GameConfiguration config,
		IAssetLoader<GameObject> loader,
		Transform parent)
	{
		_shotPool = new EffectPoolFacade(loader, config.Effects.ShotVfx, parent);
		_asteroidPool = new EffectPoolFacade(loader, config.Effects.AsteroidExplosionVfx, parent);
		_ufoPool = new EffectPoolFacade(loader, config.Effects.UfoExplosionVfx, parent);

		events.OnSpaceshipShot.Subscribe(async position =>
		{
			var fx = await _shotPool.GetAsync();
			fx.transform.position = position;
			UniTask.Void(async () =>
			{
				await UniTask.Delay(500);
				_shotPool.Release(fx);
			});
		}).AddTo(_disposables);

		events.OnEnemyKilled.Subscribe(async tuple =>
		{
			var pool = tuple.Item2 == EnemyType.UFO ? _ufoPool : _asteroidPool;
			var fx = await pool.GetAsync();
			fx.transform.position = tuple.Item1;
			UniTask.Void(async () =>
			{
				await UniTask.Delay(1000);
				pool.Release(fx);
			});
		}).AddTo(_disposables);
	}

	public void Dispose()
	{
		_disposables?.Dispose();
	}
}