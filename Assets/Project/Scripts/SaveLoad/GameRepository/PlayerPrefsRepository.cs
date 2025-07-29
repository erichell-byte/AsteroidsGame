using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace SaveLoad
{
    public class PlayerPrefsRepository : ILocalRepository
    {
        private const string GameStateKey = "GameStateKey";
        
        private Dictionary<string, string> _gameState = new();
        
        public bool TryGetData<T>(out T data)
        {
            var key = typeof(T).Name;

            if (_gameState.TryGetValue(key, out var jsonData))
            {
                data = JsonConvert.DeserializeObject<T>(jsonData);
                return true;
            }

            data = default;
            return false;
        }

        public void SetData<T>(T data)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            var key = typeof(T).Name;
            _gameState[key] = jsonData;
        }

        public UniTask LoadState()
        {
            if (PlayerPrefs.HasKey(GameStateKey))
            {
                var gameStateJson = PlayerPrefs.GetString(GameStateKey);
                _gameState = JsonConvert.DeserializeObject<Dictionary<string, string>>(gameStateJson);
            }
            else
            {
                Debug.Log("No save!");
            }

            return default;
        }
        
        public void SaveState()
        {
            AddSaveTimeToState();
            var gameStateJson = JsonConvert.SerializeObject(_gameState);
            PlayerPrefs.SetString(GameStateKey, gameStateJson);
        }

        public void AddSaveTimeToState()
        {
            SetData(new SaveTimestamp()); 
        }
    }
}