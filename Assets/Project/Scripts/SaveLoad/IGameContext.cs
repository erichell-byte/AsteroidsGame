using Zenject;

namespace SaveLoad
{
    public interface IGameContext
    {
        T GetService<T>() where T : class;
    }
    
    public class ZenjectGameContext : IGameContext
    {
        private DiContainer container;

        [Inject]
        public void Construct(DiContainer container)
        {
            this.container = container;
        }

        public T GetService<T>() where T : class
        {
            return container.Resolve<T>();
        }
    }
}