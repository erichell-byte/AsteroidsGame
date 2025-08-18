using Character;
using Cysharp.Threading.Tasks;
using Purchasing;
using SaveLoad;
using UnityEngine;
using Zenject;

namespace SaveLoad
{
    public class SaveSystemFacade
    {
        private ISaveSystem _saveSystem;
        private SpaceshipModel _spaceshipModel;
        private IPurchaseService _purchaseService;
        
        [Inject]
        public void Construct(
            ISaveSystem saveSystem,
            IPurchaseService purchaseService,
            SpaceshipModel spaceshipModel)
        {
            _saveSystem = saveSystem;
            _purchaseService = purchaseService;
            _spaceshipModel = spaceshipModel;
        }

        public UniTask SaveSpaceShipDataAsync()
        {
            var spaceShipData = ConvertToSpaceshipData();
            return _saveSystem.SaveAsync(spaceShipData);
        }
        
        public async UniTask LoadSpaceShipDataAsync()
        {
            var spaceshipData = await _saveSystem.LoadAsync<SpaceshipData>();
            SetupSpaceshipData(spaceshipData);
        }
        
        public UniTask SavePurchasedDataAsync()
        {
            var purchasedData = ConvertToPurchasedData();
            return _saveSystem.SaveAsync(purchasedData);
        }
        
        public async UniTask LoadPurchasedDataAsync()
        {
            var purchasedData = await _saveSystem.LoadAsync<PurchasedData>();
            SetupPurchasedData(purchasedData);
        }
        
        private SpaceshipData ConvertToSpaceshipData()
        {
            return new SpaceshipData()
            {
                PositionX = _spaceshipModel.Position.Value.x,
                PositionY = _spaceshipModel.Position.Value.y,
                RotationZ = _spaceshipModel.Rotation.Value,
                Timestamp = System.DateTime.UtcNow.Ticks
            };
        }
        
        private void SetupSpaceshipData(SpaceshipData data)
        {
            _spaceshipModel.SetPosition(new Vector2(data.PositionX, data.PositionY));
            _spaceshipModel.SetRotation(data.RotationZ);
        }
        
        private PurchasedData ConvertToPurchasedData()
        {
            var purchasedData = _purchaseService.GetPurchasedData();
            return new PurchasedData()
            {
                NoAds = purchasedData.NoAds
            };
        }
        
        private void SetupPurchasedData(PurchasedData data)
        {
            _purchaseService.SetPurchasedData(data);
        }
    }
}