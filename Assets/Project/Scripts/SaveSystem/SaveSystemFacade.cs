using System;
using Character;
using Cysharp.Threading.Tasks;
using Purchasing;
using Systems;
using UnityEngine;
using Zenject;

namespace SaveLoad
{
	public class SaveSystemFacade : IDisposable, IGameFinishListener
	{
		private IPurchaseService _purchaseService;
		private ISaveSystem _saveSystem;
		private SpaceshipModel _spaceshipModel;

		public void Dispose()
		{
			_purchaseService.OnPurchasedAction -= SavePurchasedData;
		}

		[Inject]
		public void Construct(
			ISaveSystem saveSystem,
			IPurchaseService purchaseService,
			SpaceshipModel spaceshipModel)
		{
			_saveSystem = saveSystem;
			_purchaseService = purchaseService;
			_spaceshipModel = spaceshipModel;

			_purchaseService.OnPurchasedAction += SavePurchasedData;
		}
		
		public void OnFinishGame()
		{
			SaveSpaceShipData().Forget();
		}

		public async UniTask SaveSpaceShipData()
		{
			var spaceShipData = ConvertToSpaceshipData();
			await _saveSystem.SaveAsync(spaceShipData);
		}

		public async UniTask LoadSpaceShipData()
		{
			var spaceshipData = await _saveSystem.LoadAsync<SpaceshipData>();
			SetupSpaceshipData(spaceshipData);
		}

		public async void SavePurchasedData(TypeOfPurchase type)
		{
			var purchasedData = ConvertToPurchasedData();
			await _saveSystem.SaveAsync(purchasedData);
		}

		public async UniTask LoadPurchasedData()
		{
			var purchasedData = await _saveSystem.LoadAsync<PurchasedData>();
			SetupPurchasedData(purchasedData);
		}

		private SpaceshipData ConvertToSpaceshipData()
		{
			return new SpaceshipData
			{
				PositionX = _spaceshipModel.Position.Value.x,
				PositionY = _spaceshipModel.Position.Value.y,
				RotationZ = _spaceshipModel.Rotation.Value,
				Timestamp = DateTime.UtcNow.Ticks
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
			return new PurchasedData
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