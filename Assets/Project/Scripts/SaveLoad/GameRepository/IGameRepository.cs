using System;
using Cysharp.Threading.Tasks;

namespace SaveLoad
{
    public interface IGameRepository
    {
        public bool TryGetData<T>(out T data);
        
        public void SetData<T>(T data);
        
        public void SaveState();
        
        public UniTask LoadState();

        public void AddSaveTimeToState();
    }
}