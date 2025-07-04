using System;
using Character;
using GameAdvertisement;
using UniRx;
using Zenject;

namespace Systems
{
    public class GameEventsController : IInitializable, IDisposable
    {
        private GameCycle gameCycle;
        private SpaceshipController spaceship;
        private IAdService adService;
        
        private CompositeDisposable disposables = new ();
        
        [Inject]
        private void Construct(
            GameCycle gameCycle,
            SpaceshipController spaceship,
            IAdService adService)
        {
            this.gameCycle = gameCycle;
            this.spaceship = spaceship;
            this.adService = adService;
        }
        
        public void Initialize()
        {
            spaceship.SpaceshipModel.IsDead
                .Where(isDead => isDead == false)
                .Subscribe(_ => gameCycle.StartGame())
                .AddTo(disposables);
            
            spaceship.SpaceshipModel.IsDead
                .Where(isDead => isDead)
                .Subscribe(_ => gameCycle.FinishGame())
                .AddTo(disposables);

            spaceship.IsCollisionWithEnemy
                .Subscribe(_ => gameCycle.PauseGame())
                .AddTo(disposables);
            
            adService.OnRewardedAdShowCompleted
                .Where(adPlace => AdPlace.GameOver == adPlace)
                .Subscribe(_ => gameCycle.ResumeGame())
                .AddTo(disposables);

            Observable.Merge(
                    adService.OnRewardedAdShowFailed,
                    adService.OnInterstitialAdShowFailed,
                    adService.OnInterstitialAdShowCompleted)
                .Where(adPlace => AdPlace.GameOver == adPlace)
                .Subscribe(_ => gameCycle.FinishGame())
                .AddTo(disposables);
        }
        
        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}