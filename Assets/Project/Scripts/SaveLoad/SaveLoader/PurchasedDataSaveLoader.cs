using Purchasing;
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

        public string GetSavedDataName()
        {
            return nameof(PurchasedData);
        }

        private PurchasedData ConvertToData(IPurchaseService service)
        {
            var purchasedData = service.GetPurchasedData();
            return new PurchasedData()
            {
                noAds = purchasedData.noAds
            };
        }

        private void SetupData(PurchasedData data)
        {
            service.SetPurchasedData(data);
        }

        private void SetupDefaultData()
        {
            service.SetPurchasedData(new PurchasedData { noAds = false });
        }
        
    }
}