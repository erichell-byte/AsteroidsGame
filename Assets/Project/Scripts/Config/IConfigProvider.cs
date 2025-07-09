using Cysharp.Threading.Tasks;

namespace Config
{
    public interface IConfigProvider
    {
        UniTask FetchAndActivateAsync();

        public RemoteConfig GetRemoteConfig();
    }
}