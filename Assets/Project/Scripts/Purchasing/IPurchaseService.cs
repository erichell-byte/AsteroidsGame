using System;
using SaveLoad;
using UnityEngine.Purchasing;

namespace Purchasing
{
	public interface IPurchaseService : IDetailedStoreListener, IStoreController
	{
		public event Action<TypeOfPurchase> OnPurchasedAction;

		public PurchasedData GetPurchasedData();

		public void SetPurchasedData(PurchasedData data);
	}
}