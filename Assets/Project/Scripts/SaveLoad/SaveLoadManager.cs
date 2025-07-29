using UnityEngine;
using Zenject;

namespace SaveLoad
{
    public class SaveLoadManager
    {
        private ISaveLoader[] _saveLoaders;
        private ILocalRepository _localRepository;
        private IRemoteRepository _remoteRepository;

        [Inject]
        private void Construct(ILocalRepository localRepository,
            IRemoteRepository remoteRepository,
            ISaveLoader[] saveLoaders)
        {
            this._localRepository = localRepository;
            this._remoteRepository = remoteRepository;
            this._saveLoaders = saveLoaders;
        }
        
        public void SaveGame()
        {
            foreach (var saveLoader in _saveLoaders)
            {
                saveLoader.SaveGame(_localRepository);
                saveLoader.SaveGame(_remoteRepository);
            }
            
            _localRepository.SaveState();
            _remoteRepository.SaveState();
            
            Debug.Log("Game saved");
        }
        
        public async void LoadGame()
        {
            await _localRepository.LoadState();
            await _remoteRepository.LoadState();

            IGameRepository relevantRepository = IsRemoteDataRelevant() ? _remoteRepository : _localRepository;

            foreach (var saveLoader in _saveLoaders)
            {
                saveLoader.LoadGame(relevantRepository);
            }

            SaveGame();
            Debug.Log("Game loaded");
        }

        private bool IsRemoteDataRelevant()
        {
            if (_localRepository.TryGetData<SaveTimestamp>(out var localTimestamp) &&
                _remoteRepository.TryGetData<SaveTimestamp>(out var remoteTimestamp))
            {
                return remoteTimestamp.ticks >= localTimestamp.ticks;
            }

            if (_remoteRepository.TryGetData<SaveTimestamp>(out _))
                return true;

            return false;
        }
    }
}