using Character;
using Systems;
using Zenject;

namespace UI
{
    public class UIController : IGameStartListener
    {
        private GameUIView gameUIView;

        private SpaceshipModel spaceshipModel;
        private GameUIViewModel viewModel;
        
        [Inject]
        private void Construct(
            SpaceshipController shipController,
            GameUIView gameUIView,
            GameCycle gameCycle)
        {
            spaceshipModel = shipController.SpaceshipModel;
            this.gameUIView = gameUIView;
            
            gameCycle.AddListener(this);
        }

        public void OnStartGame()
        {
            gameUIView.Dispose();
            viewModel = new GameUIViewModel(spaceshipModel);
            gameUIView.Initialize(viewModel);
        }
    }
}