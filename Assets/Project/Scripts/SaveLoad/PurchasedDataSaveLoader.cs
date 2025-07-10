using Project.Scripts.Purchasing;
using SaveLoad.GameRepository;
using Zenject;

namespace SaveLoad
{
    public class PurchasedDataSaveLoader : ISaveLoader
    {
        private IPurchaseService service;

        [Inject]
        private void Construct(IPurchaseService service)
        {
            this.service = service;
        }
        
        public void SaveGame(IGameRepository repository)
        {
            PurchasedData data = ConvertToData(service);
            repository.SetData(data);
        }

        public void LoadGame(IGameRepository repository)
        {
            if (repository.TryGetData(out PurchasedData data))
            {
                SetupData(data);
            }
            else
            {
                SetupDefaultData();
            }
        }

        private PurchasedData ConvertToData(IPurchaseService model)
        {
            var purchasedData = model.GetPurchasedData();
            return new PurchasedData()
            {
                noAds = purchasedData.noAds
            };
        }

        private void SetupData(PurchasedData data)
        {
            service.SetPurchasedData(data);
        }

        protected void SetupDefaultData()
        {
            service.SetPurchasedData(new PurchasedData { noAds = false });
        }
        
    }
}