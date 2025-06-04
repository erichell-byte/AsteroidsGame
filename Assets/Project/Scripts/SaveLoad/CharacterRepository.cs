using UnityEngine;
using Zenject;

namespace SaveLoad
{
    public struct CharacterStats
    {
        public Vector3 position;
        public float rotationZ;
        // public List<Enemy> enemies; //TODO
    }
    
    public class CharacterRepository : IRepository<CharacterStats>
    {
        private const string PLAYER_PREFS_KEY = "CharacterStats";
        private IStorageService storageService;

        [Inject]
        public void Construct(IStorageService storageService)
        {
            this.storageService = storageService;
        }

        public bool Load(out CharacterStats data)
        {
            if (storageService.HasKey(PLAYER_PREFS_KEY))
            {
                var json = storageService.GetString(PLAYER_PREFS_KEY);
                data = JsonUtility.FromJson<CharacterStats>(json);
                return true;
            }

            data = default;
            return false;
        }

        public void Save(CharacterStats data)
        {
            var json = JsonUtility.ToJson(data);
            storageService.SetString(PLAYER_PREFS_KEY, json);
        }
    }
}