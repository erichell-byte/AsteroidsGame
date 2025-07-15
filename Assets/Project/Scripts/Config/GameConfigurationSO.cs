using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Config
{
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "ScriptableObjects/GameConfiguration", order = 0)]
    public class GameConfigurationSO : ScriptableObject
    {
        [Header("Remote Configuration")]
        public RemoteConfig remoteConfig;
        
        [Header("Addressable Assets Configuration")]
        public AssetReferenceGameObject bulletId;
        public AssetReferenceGameObject asteroidId;
        public AssetReferenceGameObject asteroidSmallId;
        public AssetReferenceGameObject ufoId;
        
        [Header("Enemy Configuration")]
        public List<EnemyConfig> enemiesConfigs;
        
        [Space(20)]
        [Header("Advertisement Configuration")]
        public string androidGameId;
        public string iOSGameId;
        public bool adTestMode;
        
        [Space(20)]
        [Header("Purchasing Configuration")]
        public string noAdsProductId;
    }
}