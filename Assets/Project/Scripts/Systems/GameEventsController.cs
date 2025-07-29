using System;
using Character;
using GameAdvertisement;
using UniRx;
using Zenject;

namespace Systems
{
    public class GameEventsController : IInitializable, IDisposable
    {
        private readonly CompositeDisposable _disposables = new ();
        
        private GameCycle _gameCycle;
        private SpaceshipController _spaceship;
        private IAdService _adService;
        private IGameEvents _gameEvents;
        
        [Inject]
        private void Construct(
            GameCycle gameCycle,
            SpaceshipController spaceship,
            IAdService adService,
            IGameEvents gameEvents)
        {
            this._gameCycle = gameCycle;
            this._spaceship = spaceship;
            this._adService = adService;
            this._gameEvents = gameEvents;
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

            Observable.Merge(
                    _adService.OnRewardedAdShowFailed,
                    _adService.OnInterstitialAdShowFailed,
                    _adService.OnInterstitialAdShowCompleted)
                .Where(adPlace => AdPlace.GameOver == adPlace)
                .Subscribe(_ => _gameCycle.FinishGame())
                .AddTo(_disposables);
        }
        
        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}