using System.Reflection;
using UnityEngine;
using Zenject;

namespace Config
{
    public class GameConfigApplier : IInitializable
    {
        private readonly IConfigProvider configProvider;
        private readonly GameConfiguration gameConfig;

        [Inject]
        public GameConfigApplier(IConfigProvider configProvider, GameConfiguration gameConfig)
        {
            this.configProvider = configProvider;
            this.gameConfig = gameConfig;
        }

        public async void Initialize()
        {
            await configProvider.FetchAndActivateAsync();

            var type = gameConfig.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(float)) {
                    if (configProvider.TryGetValue(field.Name, out float remoteValue)) {
                        field.SetValue(gameConfig, remoteValue);
                    }
                }
                else if (field.FieldType == typeof(int))
                {
                    if (configProvider.TryGetValue(field.Name, out float remoteValue))
                    {
                        field.SetValue(gameConfig, Mathf.RoundToInt(remoteValue));
                    }
                }
            }
        }
    }
}