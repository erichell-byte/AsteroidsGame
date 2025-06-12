using SaveLoad;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Systems
{
    public class ApplicationManager : IInitializable
    {
        private SaveLoadManager saveLoadManager;

        [Inject]
        private void Construct(SaveLoadManager saveLoadManager)
        {
            this.saveLoadManager = saveLoadManager;
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


        public void Initialize()
        {
            saveLoadManager.LoadGame();
            LoadGame();
        }
    }
}
