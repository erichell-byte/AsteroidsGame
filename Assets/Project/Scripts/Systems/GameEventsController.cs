using System;
using Character;
using GameAdvertisement;
using UniRx;
using Zenject;

namespace Systems
{
	public class GameEventsController : IInitializable, IDisposable
	{
		private readonly CompositeDisposable _disposables = new();
		private IAdService _adService;

		private GameCycle _gameCycle;
		private IGameEvents _gameEvents;
		private SpaceshipController _spaceship;

		[Inject]
		private void Construct(
			GameCycle gameCycle,
			SpaceshipController spaceship,
			IAdService adService,
			IGameEvents gameEvents)
		{
			_gameCycle = gameCycle;
			_spaceship = spaceship;
			_adService = adService;
			_gameEvents = gameEvents;
		}

		public void Dispose()
		{
			_disposables?.Dispose();
		}

		public void Initialize()
		{
			_spaceship.SpaceshipModel.IsDead
				.Where(isDead => isDead == false)
				.Subscribe(_ => _gameCycle.StartGame())
				.AddTo(_disposables);

			_spaceship.SpaceshipModel.IsDead
				.Where(isDead => isDead)
				.Subscribe(_ => _gameCycle.FinishGame())
				.AddTo(_disposables);

			_gameEvents.OnSpaceshipCollidedWithEnemy
				.Subscribe(_ => _gameCycle.PauseGame())
				.AddTo(_disposables);

			_adService.OnRewardedAdShowCompleted
				.Where(adPlace => AdPlace.GameOver == adPlace)
				.Subscribe(_ => _gameCycle.ResumeGame())
				.AddTo(_disposables);

			_adService.OnSkipInterstitialAdBecauseNoAdsPurchased
				.Subscribe(_ => _gameCycle.FinishGame())
				.AddTo(_disposables);

			_adService.OnRewardedAdShowFailed.Merge(_adService.OnInterstitialAdShowFailed,
					_adService.OnInterstitialAdShowCompleted)
				.Where(adPlace => AdPlace.GameOver == adPlace)
				.Subscribe(_ => _gameCycle.FinishGame())
				.AddTo(_disposables);
		}
	}
}