using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.CloudSave;
using Zenject;

namespace SaveLoad
{
    public class UnityCloudSaveRepository : IRemoteRepository
    {
        private readonly Dictionary<string, object> _gameState = new();

        [Inject]
        private void Construct(ISaveLoader[] saveLoaders)
        {
            foreach (var loader in saveLoaders)
            {
                string key = loader.GetSavedDataName();
                if (!_gameState.ContainsKey(key))
                    _gameState[key] = null;
            }
        }

        public bool TryGetData<T>(out T data)
        {
            var key = typeof(T).Name;
            
            if (_gameState.TryGetValue(key, out var jsonData) && jsonData is string jsonString)
            {
                data = JsonConvert.DeserializeObject<T>(jsonString);
                return true;
            }

            data = default;
            return false;
        }

        public void SetData<T>(T data)
        {
            var key = typeof(T).Name;
            _gameState[key] = data;
        }

        public async void SaveState()
        {
            AddSaveTimeToState();

            var toSave = _gameState.ToDictionary(
                kvp => kvp.Key,
                kvp => (object)JsonConvert.SerializeObject(kvp.Value)
            );

            try
            {
                await CloudSaveService.Instance.Data.Player.SaveAsync(toSave);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"[UnityCloudSaveRepository] Ошибка сохранения в облако: {e.Message}");
            }
        }

        public async UniTask LoadState()
        {
            try
            {
                var saved = await CloudSaveService.Instance.Data.Player.LoadAllAsync();
                foreach (var pair in saved)
                {
                    _gameState[pair.Key] = pair.Value.Value;
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"[UnityCloudSaveRepository] Ошибка загрузки из облака: {e.Message}");
            }
        }

        public void AddSaveTimeToState()
        {
            SetData(new SaveTimestamp());
        }
    }
}