using Purchasing;
using Zenject;

namespace SaveLoad
{
    public class PurchasedDataSaveLoader : ISaveLoader
    {
        private IPurchaseService _service;

        [Inject]
        private void Construct(IPurchaseService service)
        {
            _service = service;
        }
        
        public void SaveGame(IGameStorage storage)
        {
            PurchasedData data = ConvertToData(_service);
            storage.SetData(data);
        }

        public void LoadGame(IGameStorage storage)
        {
            if (storage.TryGetData(out PurchasedData data))
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
                NoAds = purchasedData.NoAds
            };
        }

        private void SetupData(PurchasedData data)
        {
            _service.SetPurchasedData(data);
        }

        private void SetupDefaultData()
        {
            _service.SetPurchasedData(new PurchasedData { NoAds = false });
        }
        
    }
}