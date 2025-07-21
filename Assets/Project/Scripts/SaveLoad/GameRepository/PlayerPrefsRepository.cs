using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace SaveLoad
{
    public class PlayerPrefsRepository : ILocalRepository
    {
        private const string GAME_STATE_KEY = "GameStateKey";
        
        private Dictionary<string, string> gameState = new();
        
        public bool TryGetData<T>(out T data)
        {
            var key = typeof(T).Name;

            if (gameState.TryGetValue(key, out var jsonData))
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
            gameState[key] = jsonData;
        }

        public UniTask LoadState()
        {
            if (PlayerPrefs.HasKey(GAME_STATE_KEY))
            {
                var gameStateJson = PlayerPrefs.GetString(GAME_STATE_KEY);
                gameState = JsonConvert.DeserializeObject<Dictionary<string, string>>(gameStateJson);
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
            var gameStateJson = JsonConvert.SerializeObject(gameState);
            PlayerPrefs.SetString(GAME_STATE_KEY, gameStateJson);
        }

        public void AddSaveTimeToState()
        {
            SetData(new SaveTimestamp()); 
        }
    }
}