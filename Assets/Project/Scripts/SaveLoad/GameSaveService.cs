using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace SaveLoad
{
    public class GameSaveService
    {
        private ISaveLoader[] _saveLoaders;
        private ILocalStorage _localStorage;
        private IRemoteStorage _remoteStorage;

        [Inject]
        private void Construct(ILocalStorage localStorage,
            IRemoteStorage remoteStorage,
            ISaveLoader[] saveLoaders)
        {
            _localStorage = localStorage;
            _remoteStorage = remoteStorage;
            _saveLoaders = saveLoaders;
        }
        
        public void SaveGame()
        {
            foreach (var saveLoader in _saveLoaders)
            {
                saveLoader.SaveGame(_localStorage);
                saveLoader.SaveGame(_remoteStorage);
            }
            
            var timestamp = new SaveTimestamp();
            
            _localStorage.SetData(timestamp);
            _remoteStorage.SetData(timestamp);
            
            _localStorage.SaveState();
            _remoteStorage.SaveState();
            
            Debug.Log("Game saved");
        }
        
        public async UniTask LoadGame()
        {
            await _localStorage.LoadState();
            await _remoteStorage.LoadState();

            IGameStorage relevantStorage = IsRemoteDataRelevant() ? _remoteStorage : _localStorage;

            foreach (var saveLoader in _saveLoaders)
            {
                saveLoader.LoadGame(relevantStorage);
            }

            SaveGame();
            Debug.Log("Game loaded");
        }

        private bool IsRemoteDataRelevant()
        {
            if (_localStorage.TryGetData<SaveTimestamp>(out var localTimestamp) &&
                _remoteStorage.TryGetData<SaveTimestamp>(out var remoteTimestamp))
            {
                return remoteTimestamp.ticks >= localTimestamp.ticks;
            }

            if (_remoteStorage.TryGetData<SaveTimestamp>(out _))
                return true;

            return false;
        }
    }
}