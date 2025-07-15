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

            gameConfigSO.remoteConfig = remote;
        }
    }
}