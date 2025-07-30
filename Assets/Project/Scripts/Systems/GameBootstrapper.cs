using SaveLoad;
using Zenject;

namespace Systems
{
    public class GameBootstrapper : IInitializable
    {
        private readonly UnityServicesInitializator _unityServicesInitializator = new ();
        
        private GameSaveService _gameSaveService;
        private ISceneLoader _sceneLoader;
        
        [Inject]
        private void Construct(
            GameSaveService gameSaveService,
            ISceneLoader sceneLoader)
        {
            _gameSaveService = gameSaveService;
            _sceneLoader = sceneLoader;
        }
            
        private void LoadScene()
        {
            _sceneLoader.LoadNextScene();
        }
        
        public async void Initialize()
        {
            await _unityServicesInitializator.SetupAndSignIn();
            await _gameSaveService.LoadGame();
            LoadScene();
        }
    }
}
