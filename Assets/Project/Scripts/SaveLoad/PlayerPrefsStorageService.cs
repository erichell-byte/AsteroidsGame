using UnityEngine;

namespace SaveLoad
{
    public class PlayerPrefsStorageService : IStorageService
    {
        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public string GetString(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        public void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }
    }
}