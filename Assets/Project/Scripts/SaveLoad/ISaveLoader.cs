using SaveLoad.GameRepository;

namespace SaveLoad
{
    public interface ISaveLoader
    {
        public void SaveGame(IGameRepository repository);
        
        public void LoadGame(IGameRepository repository);
    }
}