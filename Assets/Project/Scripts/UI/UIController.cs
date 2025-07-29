using Character;
using GameAdvertisement;
using Purchasing;
using Systems;
using Zenject;

namespace UI
{
    public class UIController : IGameStartListener, IGamePauseListener, IGameResumeListener, IGameFinishListener
    {
        private SpaceshipModel _spaceshipModel;
        private IAdService _adService;
        private IPurchaseService _purchaseService;
        private GameUIView _gameUIView;
        private AdView _adView;
        
        private GameUIViewModel _gameViewModel;
        private AdViewModel _adViewModel;
        
        [Inject]
        private void Construct(
            SpaceshipController shipController,
            GameUIView gameUIView,
            AdView adView,
            GameCycle gameCycle,
            IAdService adService,
            IPurchaseService purchaseService)
        {
            _spaceshipModel = shipController.SpaceshipModel;
            _gameUIView = gameUIView;
            _adView = adView;
            _adService = adService;
            _purchaseService = purchaseService;
            
            gameCycle.AddListener(this);
        }

        public void OnStartGame()
        {
            _gameUIView.Dispose();
            _gameViewModel = new GameUIViewModel(_spaceshipModel);
            _gameUIView.Initialize(_gameViewModel);
        }

        public void OnPauseGame()
        {
            _gameUIView.Hide();
            _adViewModel = new AdViewModel(_adService, _purchaseService);
            _adView.Initialize(_adViewModel);
            _adView.Show();
        }

        public void OnResumeGame()
        {
            _gameUIView.Show();
            _adView.Hide();
            _adView.Dispose();
        }

        public void OnFinishGame()
        {
            _adView.Hide();
            _gameUIView.Show();
        }
    }
}