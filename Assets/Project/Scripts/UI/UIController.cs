using Character;
using Systems;
using Zenject;

namespace UI
{
    public class UIController : IGameStartListener
    {
        private GameUIView gameUIView;

        private CharacterModel characterModel;
        private GameUIViewModel viewModel;
        
        [Inject]
        private void Construct(
            SpaceshipController shipController,
            GameUIView gameUIView,
            GameCycle gameCycle)
        {
            characterModel = shipController.CharacterModel;
            this.gameUIView = gameUIView;
            
            gameCycle.AddListener(this);
        }

        public void OnStartGame()
        {
            gameUIView.Dispose();
            viewModel = new GameUIViewModel(characterModel);
            gameUIView.Initialize(viewModel);
        }
    }
}