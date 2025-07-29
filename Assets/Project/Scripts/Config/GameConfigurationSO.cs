using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace Config
{
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "ScriptableObjects/GameConfiguration", order = 0)]
    public class GameConfiguration : ScriptableObject
    {
        public AssetReferenceGameObject BulletId;
        public AssetReferenceGameObject AsteroidId;
        public AssetReferenceGameObject AsteroidSmallId;
        public AssetReferenceGameObject UfoId;
        
        [Header("Enemy Configuration")]
        public List<EnemyConfig> EnemiesConfigs;
        
        [Space(20)]
        public string AndroidGameId;
        public string IOSGameId;
        public bool AdTestMode;
        
        [Space(20)]
        [Header("Purchasing Configuration")]
        public string NoAdsProductId;
    }
}