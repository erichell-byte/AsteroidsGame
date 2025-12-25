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

		private RemoteConfig _parsedConfig = new();

		public RemoteConfig GetRemoteConfig()
		{
			return _parsedConfig;
		}

		public async void Initialize()
		{
			var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
			if (dependencyStatus != DependencyStatus.Available)
			{
				Debug.LogError("Firebase dependencies error: " + dependencyStatus);
				return;
			}

			var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
			await remoteConfig.FetchAsync(TimeSpan.Zero);
			await remoteConfig.ActivateAsync();
			Debug.Log("Firebase Remote Config fetched and activated.");

			var jsonString = remoteConfig.GetValue(_configKey).StringValue;
			try
			{
				if (!string.IsNullOrWhiteSpace(jsonString))
				{
					// Keep the same instance so all injected references update.
					JsonConvert.PopulateObject(jsonString, _parsedConfig);
				}
				else
				{
					Debug.LogWarning("Remote config is empty. Using default values.");
				}
			}
			catch (Exception e)
			{
				Debug.LogError("Failed to parse remote config: " + e);
			}
		}
	}
}
