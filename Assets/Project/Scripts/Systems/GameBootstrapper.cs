using SaveLoad;
using Zenject;

namespace Systems
{
	public class GameBootstrapper : IInitializable
	{
		private readonly UnityServicesInitializator _unityServicesInitializator = new();

		private SaveSystemFacade _saveSystemFacade;
		private ISceneLoader _sceneLoader;

		[Inject]
		private void Construct(
			SaveSystemFacade gameSaveService,
			ISceneLoader sceneLoader)
		{
			_saveSystemFacade = gameSaveService;
			_sceneLoader = sceneLoader;
		}

		public async void Initialize()
		{
			await _unityServicesInitializator.SetupAndSignIn();
			await _saveSystemFacade.LoadPurchasedData();
			await _saveSystemFacade.LoadSpaceShipData();
			LoadScene();
		}

		private void LoadScene()
		{
			_sceneLoader.LoadNextScene();
		}
	}
}