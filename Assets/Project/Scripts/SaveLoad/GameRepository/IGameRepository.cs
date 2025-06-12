namespace SaveLoad.GameRepository
{
    public interface IGameRepository
    {
        public bool TryGetData<T>(out T data);
        
        public void SetData<T>(T data);
        
        public void SaveState();
        
        public void LoadState();
    }
}