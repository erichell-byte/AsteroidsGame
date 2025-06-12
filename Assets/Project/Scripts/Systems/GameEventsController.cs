using System;
using Character;
using UniRx;
using Zenject;

namespace Systems
{
    public class GameEventsController : IInitializable, IDisposable
    {
        private GameCycle gameCycle;
        private SpaceshipController spaceship;
        
        private CompositeDisposable disposables = new ();
        
        [Inject]
        private void Construct(
            GameCycle gameCycle,
            SpaceshipController spaceship)
        {
            this.gameCycle = gameCycle;
            this.spaceship = spaceship;
        }
        
        public void Initialize()
        {
            spaceship.SpaceshipModel.IsDead
                .Where(isDead => isDead == false)
                .Subscribe(_ => gameCycle.StartGame())
                .AddTo(disposables);
            
            spaceship.SpaceshipModel.IsDead
                .Where(isDead => isDead == true)
                .Subscribe(_ => gameCycle.FinishGame())
                .AddTo(disposables);
        }
        
        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}