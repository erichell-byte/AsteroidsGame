using System;
using System.Collections.Generic;
using Config;
using SaveLoad;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using Zenject;

namespace Purchasing
{
    public enum TypeOfPurchase
    {
        NoAds,
    }
    public class UnityPurchaseService : IPurchaseService, IInitializable
    {
        private IStoreController _controller;
        private IExtensionProvider _extensions;
        private GameSaveService _gameSaveService;
        
        private PurchasedData _purchasedData = new ();
        private string _noAdsProductId;
        
        public ProductCollection products { get; }
        public event Action<TypeOfPurchase> OnPurchasedAction;
        
        [Inject]
        private void Construct(
            GameConfiguration gameConfig,
            GameSaveService gameSaveService)
        {
            _noAdsProductId = gameConfig.NoAdsProductId;
            _gameSaveService = gameSaveService;
        }
        
        public void Initialize()
        {
            InitializeUnityServices();
            InitializePurchasing();
        }

        private async void InitializeUnityServices()
        {
            try
            {
                var options = new InitializationOptions()
                    .SetEnvironmentName("production");

                await UnityServices.InitializeAsync(options);
            }
            catch (Exception exception)
            {
                Debug.LogError("Failed to initialize Unity Services: " + exception.Message);
            }
        }
        
        private void InitializePurchasing()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            builder.AddProduct(_noAdsProductId, ProductType.NonConsumable);
            UnityPurchasing.Initialize(this, builder);
        }
        
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogError("Purchase initialization failed: " + error);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.LogError("Purchase initialization failed: " + error + ", message: " + message);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            if (purchaseEvent.purchasedProduct.definition.id == _noAdsProductId)
            {
                Debug.Log("No ads product purchased successfully: " + purchaseEvent.purchasedProduct.definition.id);
                _purchasedData.NoAds = true;
                OnPurchasedAction?.Invoke(TypeOfPurchase.NoAds);
                
            }
            
            _gameSaveService.SaveGame();
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.LogError($"Purchase failed for product {product.definition.id}: {failureReason}");
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            this._controller = controller;
            this._extensions = extensions;
            
            Debug.Log("Unity purchase service initialized");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            Debug.LogError($"Purchase failed for product {product.definition.id}: {failureDescription.reason}");
        }
        

        public void InitiatePurchase(Product product, string payload)
        {
            if (product == null)
            {
                Debug.LogError("Product is null, cannot initiate purchase.");
                return;
            }

            if (_controller == null)
            {
                Debug.LogError("StoreController is not initialized.");
                return;
            }

            _controller.InitiatePurchase(product, payload);
        }

        public void InitiatePurchase(string productId, string payload)
        {
            if (string.IsNullOrEmpty(productId))
            {
                Debug.LogError("ProductId is empty, cannot initiate purchase.");
                return;
            }

            if (_controller == null)
            {
                Debug.LogError("StoreController is not initialized.");
                return;
            }

            _controller.InitiatePurchase(productId, payload);
        }

        public void InitiatePurchase(Product product)
        {
            if (product == null)
            {
                Debug.LogError("Product is null, cannot initiate purchase.");
                return;
            }

            if (_controller == null)
            {
                Debug.LogError("StoreController is not initialized.");
                return;
            }

            _controller.InitiatePurchase(product);
        }

        public void InitiatePurchase(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                Debug.LogError("ProductId is empty, cannot initiate purchase.");
                return;
            }

            if (_controller == null)
            {
                Debug.LogError("StoreController is not initialized.");
                return;
            }

            _controller.InitiatePurchase(productId);
        }

        public void FetchAdditionalProducts(HashSet<ProductDefinition> additionalProducts, Action successCallback, Action<InitializationFailureReason> failCallback)
        {
            
        }

        public void FetchAdditionalProducts(HashSet<ProductDefinition> additionalProducts, Action successCallback, Action<InitializationFailureReason, string> failCallback)
        {
            
        }

        public void ConfirmPendingPurchase(Product product)
        {
            
            if (_controller == null)
            {
                Debug.LogError("StoreController is not initialized.");
                return;
            }

            _controller.ConfirmPendingPurchase(product);
        }

        public PurchasedData GetPurchasedData()
        {
            return _purchasedData;
        }
        
        public void SetPurchasedData(PurchasedData data)
        {
            _purchasedData = data;
        }
    }
}