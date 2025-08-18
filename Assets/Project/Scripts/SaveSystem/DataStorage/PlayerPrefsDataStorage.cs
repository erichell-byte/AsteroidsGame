using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SaveLoad
{
    public sealed class PlayerPrefsDataStorage : IDataStorage
    {
        public UniTask<string> ReadAsync(string key)
        {
            string serializedData = PlayerPrefs.GetString(key);
            return UniTask.FromResult(serializedData);
        }

        public UniTask WriteAsync(string key, string serializedData)
        {
            PlayerPrefs.SetString(key, serializedData);
            return UniTask.CompletedTask;
        }

        public UniTask DeleteAsync(string key)
        {
            PlayerPrefs.DeleteKey(key);
            return UniTask.CompletedTask;
        }

        public UniTask<bool> ExistsAsync(string key)
        {
            bool exists = PlayerPrefs.HasKey(key);
            return UniTask.FromResult(exists);
        }
    }
}