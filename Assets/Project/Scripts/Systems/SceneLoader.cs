using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Systems
{
    public class SceneLoader : ISceneLoader
    {
        public async UniTask LoadNextScene()
        {
            int currentIndex = GetCurrentSceneIndex();
            await LoadSceneByIndex(currentIndex + 1);
        }

        public async UniTask LoadPreviousScene()
        {
            int currentIndex = GetCurrentSceneIndex();
            await LoadSceneByIndex(currentIndex - 1);
        }

        public async UniTask LoadFirstScene()
        {
            await LoadSceneByIndex(0);
        }

        private async UniTask LoadSceneByIndex(int index)
        {
            if (index < 0 || index >= SceneManager.sceneCountInBuildSettings)
            {
                Debug.LogWarning($"[SceneLoader] Scene index {index} is out of range.");
                return;
            }

            await SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        }

        private int GetCurrentSceneIndex()
        {
            return SceneManager.GetActiveScene().buildIndex;
        }
    }
}