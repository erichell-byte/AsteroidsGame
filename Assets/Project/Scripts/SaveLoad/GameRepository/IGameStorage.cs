using Cysharp.Threading.Tasks;

namespace SaveLoad
{
    public interface IGameStorage
    {
        public bool TryGetData<T>(out T data);
        
        public void SetData<T>(T data);
        
        public void SaveState();
        
        public UniTask LoadState();
    }
}