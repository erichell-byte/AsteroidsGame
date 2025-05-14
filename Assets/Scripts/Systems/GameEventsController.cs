using Character;
using UnityEngine;

namespace Systems
{
    public class GameEventsController : MonoBehaviour
    {
        [SerializeField] private SpaceshipController spaceship;
        [SerializeField] private ButtonView startGameButton;

        private void Start()
        {
            startGameButton.OnClick += StartGame;
            spaceship.OnShipDie += GameCycle.Instance.FinishGame;
        }

        private void StartGame()
        {
            GameCycle.Instance.StartGame();
        }

        public void OnDestroy()
        {
            startGameButton.OnClick -= StartGame;
            spaceship.OnShipDie -= GameCycle.Instance.FinishGame;
        }
    }
}