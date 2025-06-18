using SaveLoad.GameRepository;
using UnityEngine;
using Zenject;

namespace SaveLoad
{
    public class SaveLoadManager
    {
        private ISaveLoader[] saveLoaders;
        private IGameRepository gameRepository;

        [Inject]
        private void Construct(IGameRepository gameRepository, ISaveLoader[] saveLoaders)
        {
            this.gameRepository = gameRepository;
            this.saveLoaders = saveLoaders;
        }
        
        public void SaveGame()
        {
            foreach (var saveLoader in saveLoaders)
            {
                saveLoader.SaveGame(gameRepository);
            }
            
            gameRepository.SaveState();
            
            Debug.Log("Game saved");
        }
        
        public void LoadGame()
        {
            gameRepository.LoadState();
            
            foreach (var saveLoader in saveLoaders)
            {
                saveLoader.LoadGame(gameRepository);
            }
            Debug.Log("Game loaded");
        }
    }
}