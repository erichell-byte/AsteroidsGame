using Cysharp.Threading.Tasks;

namespace Config
{
    public interface IConfigProvider
    {
        UniTask FetchAndActivateAsync();
        
        bool TryGetValue(string key, out float value);
    }
}