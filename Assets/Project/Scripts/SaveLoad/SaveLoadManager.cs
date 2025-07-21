
using UnityEngine;
using Zenject;

namespace SaveLoad
{
    public class SaveLoadManager
    {
        private ISaveLoader[] saveLoaders;
        private ILocalRepository localRepository;
        private IRemoteRepository remoteRepository;

        [Inject]
        private void Construct(ILocalRepository localRepository,
            IRemoteRepository remoteRepository,
            ISaveLoader[] saveLoaders)
        {
            this.localRepository = localRepository;
            this.remoteRepository = remoteRepository;
            this.saveLoaders = saveLoaders;
        }
        
        public void SaveGame()
        {
            foreach (var saveLoader in saveLoaders)
            {
                saveLoader.SaveGame(localRepository);
                saveLoader.SaveGame(remoteRepository);
            }
            
            localRepository.SaveState();
            remoteRepository.SaveState();
            
            Debug.Log("Game saved");
        }
        
        public async void LoadGame()
        {
            await localRepository.LoadState();
            await remoteRepository.LoadState();

            IGameRepository relevantRepository = IsRemoteDataRelevant() ? remoteRepository : localRepository;

            foreach (var saveLoader in saveLoaders)
            {
                saveLoader.LoadGame(relevantRepository);
            }

            SaveGame();
            Debug.Log("Game loaded");
        }

        private bool IsRemoteDataRelevant()
        {
            if (localRepository.TryGetData<SaveTimestamp>(out var localTimestamp) &&
                remoteRepository.TryGetData<SaveTimestamp>(out var remoteTimestamp))
            {
                return remoteTimestamp.ticks >= localTimestamp.ticks;
            }

            if (remoteRepository.TryGetData<SaveTimestamp>(out _))
                return true;

            return false;
        }
    }
}