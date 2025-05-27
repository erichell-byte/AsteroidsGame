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

    public interface IGameUpdateListener : IGameListener
    {
        void OnUpdate(float deltaTime);
    }
    
    public interface IGameFixedUpdateListener : IGameListener
    {
        void OnFixedUpdate(float deltaTime);
    }
}