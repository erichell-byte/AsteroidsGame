using Character;
using GameAdvertisement;
using Purchasing;
using Systems;
using Zenject;

namespace UI
{
	public class UIController : IGameStartListener,
		IGamePauseListener,
		IGameResumeListener,
		IGameFinishListener
	{
		private IAdService _adService;
		private GameOverView _gameOverView;
		private GameOverViewModel _gameOverViewModel;
		private GameUIView _gameUIView;

		private GameUIViewModel _gameViewModel;
		private IPurchaseService _purchaseService;
		private SpaceshipModel _spaceshipModel;
		private ISceneLoader _sceneLoader;

		[Inject]
		private void Construct(
			SpaceshipController shipController,
			GameUIView gameUIView,
			GameOverView gameOverView,
			IAdService adService,
			IPurchaseService purchaseService,
			ISceneLoader sceneLoader)
		{
			_spaceshipModel = shipController.SpaceshipModel;
			_gameUIView = gameUIView;
			_gameOverView = gameOverView;
			_adService = adService;
			_purchaseService = purchaseService;
			_sceneLoader = sceneLoader;
			_gameOverViewModel = new GameOverViewModel(_adService, _purchaseService);
			_gameViewModel = new GameUIViewModel(_spaceshipModel, _sceneLoader);
			_gameUIView.Initialize(_gameViewModel);
		}

		public void OnFinishGame()
		{
			_gameOverView.Initialize(_gameOverViewModel);
			_gameOverView.Hide();
			_gameUIView.Show();
		}

		public void OnPauseGame()
		{
			_gameUIView.Hide();
			_gameOverView.Initialize(_gameOverViewModel);
			_gameOverView.Show();
		}

		public void OnResumeGame()
		{
			_gameUIView.Show();
			_gameOverView.Hide();
			_gameOverView.Dispose();
		}

		public void OnStartGame()
		{
			_gameUIView.Show();
		}
	}
}