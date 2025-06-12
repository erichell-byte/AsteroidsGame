using SaveLoad.GameRepository;
using Zenject;

namespace SaveLoad
{
    public abstract class SaveLoader<TService, TData> : ISaveLoader
    {
        private TService service;

        [Inject]
        private void Construct(TService service)
        {
            this.service = service;
        }
        
        public void SaveGame(IGameRepository repository)
        {
            TData data = ConvertToData(service);
            repository.SetData(data);
        }

        public void LoadGame(IGameRepository repository)
        {
            if (repository.TryGetData(out TData data))
            {
                SetupData(service, data);
            }
            else
            {
                SetupDefaultData(service);
            }
        }
        
        protected abstract TData ConvertToData(TService model);
        protected abstract void SetupData(TService service, TData data);

        protected virtual void SetupDefaultData(TService service) { }
    }
}