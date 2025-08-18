using System;
using Cysharp.Threading.Tasks;
using Installers;
using Zenject;

namespace SaveLoad
{
    public class SaveSystem : ISaveSystem
    {
        private ISerializer _serializer;
        private IDataStorage _localDataStorage;
        private IDataStorage _cloudDataStorage;
        private IKeysProvider _keysProvider;
        
        private SaveSystem(
            ISerializer serializer,
            [Inject(Id = StorageId.Local)] IDataStorage localDataStorage,
            [Inject(Id = StorageId.Cloud)] IDataStorage cloudDataStorage,
            IKeysProvider keysProvider)
        {
            _serializer = serializer;
            _localDataStorage = localDataStorage;
            _cloudDataStorage = cloudDataStorage;
            _keysProvider = keysProvider;
        }
        
        public async UniTask SaveAsync<TData>(TData data) where TData : ISaveData
        {
            data.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            string dataKey = _keysProvider.Provide<TData>();
            string serializedData = await _serializer.SerializeAsync(data);
            
            await _cloudDataStorage.WriteAsync(dataKey, serializedData);
            await _localDataStorage.WriteAsync(dataKey, serializedData);
        }

        public async UniTask<TData> LoadAsync<TData>() where TData : ISaveData
        {
            string dataKey = _keysProvider.Provide<TData>();
            string localSerializedData = await _localDataStorage.ReadAsync(dataKey);
            string cloudSerializedData = await _cloudDataStorage.ReadAsync(dataKey);
            
            TData localData =  !string.IsNullOrEmpty(localSerializedData) ? await _serializer.DeserializeAsync<TData>(localSerializedData) : await UniTask.FromResult<TData>(default);
            TData cloudData =  !string.IsNullOrEmpty(cloudSerializedData) ? await _serializer.DeserializeAsync<TData>(cloudSerializedData) : await UniTask.FromResult<TData>(default);
            
            if (localData == null) return cloudData;
            if (cloudData == null) return localData;

            return cloudData.Timestamp > localData.Timestamp ? cloudData : localData;
        }
    }
}