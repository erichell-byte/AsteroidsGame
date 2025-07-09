using Zenject;

namespace Config
{
    public class GameConfigApplier : IInitializable
    {
        private readonly IConfigProvider configProvider;
        private readonly GameConfigurationSO gameConfigSO;

        [Inject]
        public GameConfigApplier(IConfigProvider configProvider, GameConfigurationSO gameConfigSO)
        {
            this.configProvider = configProvider;
            this.gameConfigSO = gameConfigSO;
        }

        async void IInitializable.Initialize()
        {
            await configProvider.FetchAndActivateAsync();
            var remote = configProvider.GetRemoteConfig();
            if (remote == null) return;

            var remoteType = remote.GetType();
            var soType = gameConfigSO.GetType();

            foreach (var remoteField in remoteType.GetFields())
            {
                var soField = soType.GetField(remoteField.Name);
                if (soField != null && soField.FieldType == remoteField.FieldType)
                {
                    soField.SetValue(gameConfigSO, remoteField.GetValue(remote));
                }
            }
        }
    }
}