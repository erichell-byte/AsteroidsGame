using Character;
using UniRx;
using UnityEngine;
using Zenject;

namespace Systems
{
    public class GameEventsController : MonoBehaviour
    {
        private GameCycle gameCycle;
        private SpaceshipController spaceship;
        
        [Inject]
        private void Construct(
            GameCycle gameCycle,
            SpaceshipController spaceship)
        {
            this.gameCycle = gameCycle;
            this.spaceship = spaceship;
        }
        
        private void Awake()
        {
            spaceship.CharacterModel.IsDead
                .Where(isDead => isDead == false)
                .Subscribe(_ => StartGame())
                .AddTo(this);
            
            spaceship.CharacterModel.IsDead
                .Where(isDead => isDead == true)
                .Subscribe(_ => gameCycle.FinishGame())
                .AddTo(this);
        }

        private void StartGame()
        {
            gameCycle.StartGame();
        }
    }
}