using System;
using Cysharp.Threading.Tasks;
using Installers;
using Zenject;

namespace SaveLoad
{
	public class SaveSystem : ISaveSystem
	{
		private readonly IDataStorage _localDataStorage;
		private readonly IDataStorage _cloudDataStorage;
		private readonly IKeysProvider _keysProvider;
		private readonly ISerializer _serializer;

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
			var dataKey = _keysProvider.Provide<TData>();
			var serializedData = await _serializer.SerializeAsync(data);

			await _cloudDataStorage.WriteAsync(dataKey, serializedData);
			await _localDataStorage.WriteAsync(dataKey, serializedData);
		}

		public async UniTask<TData> LoadAsync<TData>() where TData : ISaveData
		{
			var dataKey = _keysProvider.Provide<TData>();
			var localSerializedData = await _localDataStorage.ReadAsync(dataKey);
			var cloudSerializedData = await _cloudDataStorage.ReadAsync(dataKey);

			var localData = !string.IsNullOrEmpty(localSerializedData)
				? await _serializer.DeserializeAsync<TData>(localSerializedData)
				: await UniTask.FromResult<TData>(default);
			var cloudData = !string.IsNullOrEmpty(cloudSerializedData)
				? await _serializer.DeserializeAsync<TData>(cloudSerializedData)
				: await UniTask.FromResult<TData>(default);

			if (localData == null) return cloudData;
			if (cloudData == null) return localData;

			return cloudData.Timestamp > localData.Timestamp ? cloudData : localData;
		}
	}
}