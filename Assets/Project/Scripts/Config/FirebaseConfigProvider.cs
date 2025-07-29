using System;
using Firebase;
using Firebase.RemoteConfig;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;


namespace Config
{
    public class FirebaseConfigProvider : IConfigProvider, IInitializable
    {
        private readonly string _configKey = "GameConfiguration";
        
        private RemoteConfig _parsedConfig = new ();
        
        public async void Initialize()
        {
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (dependencyStatus != DependencyStatus.Available) {
                Debug.LogError("Firebase dependencies error: " + dependencyStatus);
                return;
            }
            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            await remoteConfig.FetchAsync(TimeSpan.Zero);
            await remoteConfig.ActivateAsync();
            Debug.Log("Firebase Remote Config fetched and activated.");
            
            string jsonString = remoteConfig.GetValue(_configKey).StringValue;
            try { this._parsedConfig = JsonConvert.DeserializeObject<RemoteConfig>(jsonString); } 
            catch (Exception e) 
            {
                Debug.LogError("Failed to parse remote config: " + e);
                this._parsedConfig = new();
            }
        }
        
        public RemoteConfig GetRemoteConfig() => _parsedConfig;
        
    }
}