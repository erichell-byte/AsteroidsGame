using System;
using AssetsLoader;
using Config;
using Cysharp.Threading.Tasks;
using Enemies;
using Pools;
using Systems;
using UniRx;
using UnityEngine;
using Zenject;

public class EffectsMediator : IDisposable
{
	private readonly EffectPoolFacade _asteroidPool;
	private readonly EffectPoolFacade _shotPool;
	private readonly EffectPoolFacade _ufoPool;
	private readonly EffectsConfig _config;

	private readonly CompositeDisposable _disposables = new();

	[Inject]
	public EffectsMediator(
		IGameEvents events,
		GameConfiguration gameConfig,
		IAssetLoader<GameObject> loader,
		Transform parent)
	{
		_config = gameConfig.Effects;
		_shotPool = new EffectPoolFacade(loader, _config.ShotVfx, parent);
		_asteroidPool = new EffectPoolFacade(loader, _config.AsteroidExplosionVfx, parent);
		_ufoPool = new EffectPoolFacade(loader, _config.UfoExplosionVfx, parent);

		events.OnSpaceshipShot.Subscribe(async position =>
		{
			var fx = await _shotPool.GetAsync();
			fx.transform.position = position;
			UniTask.Void(async () =>
			{
				await UniTask.Delay(_config.ShotDuration);
				_shotPool.Release(fx);
			});
		}).AddTo(_disposables);

		events.OnEnemyKilled.Subscribe(async tuple =>
		{
			var pool = tuple.Item2 == EnemyType.UFO ? _ufoPool : _asteroidPool;
			var fx = await pool.GetAsync();
			fx.transform.position = tuple.Item1;
			int delay = tuple.Item2 == EnemyType.UFO
				? _config.UfoExplosionDuration
				: _config.AsteroidExplosionDuration;

			UniTask.Void(async () =>
			{
				await UniTask.Delay(delay);
				pool.Release(fx);
			});
		}).AddTo(_disposables);
	}

	public void Dispose()
	{
		_disposables?.Clear();
	}
}