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
        private IAdController adController;
        
        private CompositeDisposable disposables = new ();
        
        [Inject]
        private void Construct(
            GameCycle gameCycle,
            SpaceshipController spaceship,
            IAdController adController)
        {
            this.gameCycle = gameCycle;
            this.spaceship = spaceship;
            this.adController = adController;
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
            
            adController.OnRewardedAdShowCompleted
                .Subscribe(_ => gameCycle.ResumeGame())
                .AddTo(disposables);

            Observable.Merge(
                    adController.OnRewardedAdShowFailed,
                    adController.OnInterstitialAdShowFailed,
                    adController.OnInterstitialAdShowCompleted)
                .Subscribe(_ => gameCycle.FinishGame())
                .AddTo(disposables);
        }
        
        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}