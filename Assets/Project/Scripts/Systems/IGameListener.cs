namespace Systems
{
    public interface IGameListener
    {
        
    }
    
    public interface IGameStartListener : IGameListener
    {
        void OnStartGame();
    }

    public interface IGameFinishListener : IGameListener
    {
        void OnFinishGame();
    }
}