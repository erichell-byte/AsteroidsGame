using System;
using SaveLoad;
using UnityEngine.Purchasing;

namespace Project.Scripts.Purchasing
{
    public interface IPurchaseService: IDetailedStoreListener, IStoreController
    {
        public event Action NoAdsPurchased;
        public PurchasedData GetPurchasedData();

        public void SetPurchasedData(PurchasedData data);
    }
}