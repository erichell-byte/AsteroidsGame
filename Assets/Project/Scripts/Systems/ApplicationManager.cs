using SaveLoad;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Systems
{
    public class ApplicationManager : IInitializable
    {
        private readonly UnityServicesInitializator _unityServicesInitializator = new ();
        
        private SaveLoadManager _saveLoadManager;
        
        [Inject]
        private void Construct(SaveLoadManager saveLoadManager)
        {
            _saveLoadManager = saveLoadManager;
        }
            
        public void LoadGame()
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            int nextIndex = currentIndex + 1;
            if (nextIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadSceneAsync(nextIndex, LoadSceneMode.Single);
            }
            else
            {
                Debug.LogWarning("No next scene in build settings.");
            }
        }


        public async void Initialize()
        {
            await _unityServicesInitializator.SetupAndSignIn();
            _saveLoadManager.LoadGame();
            LoadGame();
        }
    }
}
