using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.RemoteConfig;
using Newtonsoft.Json;
using UnityEngine;


namespace Config
{
    public class FirebaseConfigProvider : IConfigProvider
    {
        private readonly string configKey = "GameConfiguration";
        
        private RemoteConfig parsedConfig = new ();
        
        public async UniTask FetchAndActivateAsync() {
            
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (dependencyStatus != DependencyStatus.Available) {
                Debug.LogError("Firebase dependencies error: " + dependencyStatus);
                return;
            }
            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            await remoteConfig.FetchAsync(TimeSpan.Zero);
            await remoteConfig.ActivateAsync();
            Debug.Log("Firebase Remote Config fetched and activated.");
            
            string jsonString = remoteConfig.GetValue(configKey).StringValue;
            try { this.parsedConfig = JsonConvert.DeserializeObject<RemoteConfig>(jsonString); } 
            catch (Exception e) 
            {
                Debug.LogError("Failed to parse remote config: " + e);
                this.parsedConfig = new();
            }
        }
        
        public RemoteConfig GetRemoteConfig() => parsedConfig;
    }
}