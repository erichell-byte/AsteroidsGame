using Character;
using GameAdvertisement;
using Project.Scripts.UI;
using Systems;
using Zenject;

namespace UI
{
    public class UIController : IGameStartListener, IGamePauseListener, IGameResumeListener, IGameFinishListener
    {
        private SpaceshipModel spaceshipModel;
        private IAdController adController;
        private GameUIView gameUIView;
        private AdView adView;
        
        private GameUIViewModel gameViewModel;
        private AdViewModel adViewModel;
        
        [Inject]
        private void Construct(
            SpaceshipController shipController,
            GameUIView gameUIView,
            AdView adView,
            GameCycle gameCycle,
            IAdController adController)
        {
            spaceshipModel = shipController.SpaceshipModel;
            this.gameUIView = gameUIView;
            this.adView = adView;
            this.adController = adController;
            
            gameCycle.AddListener(this);
        }

        public void OnStartGame()
        {
            gameUIView.Dispose();
            gameViewModel = new GameUIViewModel(spaceshipModel);
            gameUIView.Initialize(gameViewModel);
        }

        public void OnPauseGame()
        {
            gameUIView.Hide();
            adViewModel = new AdViewModel(adController);
            adView.Initialize(adViewModel);
            adView.Show();
        }

        public void OnResumeGame()
        {
            gameUIView.Show();
            adView.Hide();
            adView.Dispose();
        }

        public void OnFinishGame()
        {
            adView.Hide();
            gameUIView.Show();
        }
    }
}