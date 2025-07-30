namespace SaveLoad
{
    public interface ISaveLoader
    {
        public void SaveGame(IGameStorage storage);
        
        public void LoadGame(IGameStorage storage);

        public string GetSavedDataName();
    }
}