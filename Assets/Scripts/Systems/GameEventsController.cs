using Character;
using UnityEngine;
using Zenject;

namespace Systems
{
    public class GameEventsController : MonoBehaviour
    {
        private GameCycle gameCycle;
        
        [SerializeField] private SpaceshipController spaceship;
        [SerializeField] private ButtonView startGameButton;
        
        [Inject]
        private void Construct(GameCycle gameCycle)
        {
            this.gameCycle = gameCycle;
        }
        
        private void Start()
        {
            startGameButton.OnClick += StartGame;
            spaceship.OnShipDie += gameCycle.FinishGame;
        }

        private void StartGame()
        {
            gameCycle.StartGame();
        }

        public void OnDestroy()
        {
            startGameButton.OnClick -= StartGame;
            spaceship.OnShipDie -= gameCycle.FinishGame;
        }
    }
}