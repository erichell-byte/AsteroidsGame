using Cysharp.Threading.Tasks;

namespace SaveLoad
{
    public interface ISaveSystem
    {
        UniTask SaveAsync<TData>(TData data) where TData : ISaveData;  
        UniTask<TData> LoadAsync<TData>() where TData : ISaveData;
    }
}