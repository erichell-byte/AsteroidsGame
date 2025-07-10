using System;
using Config;
using Project.Scripts.Purchasing;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Project.Scripts.UI
{
    public class MenuController: IDisposable
    {
        private MenuView menuView;
        private IPurchaseService purchaseService;
        private string noAdsProductId;

        [Inject]
        private void Construct(
            MenuView menuView,
            IPurchaseService purchaseService,
            GameConfigurationSO gameConfigSO)
        {
            this.menuView = menuView;
            this.purchaseService = purchaseService;

            noAdsProductId = gameConfigSO.noAdsProductId;
            
            menuView.StartClicked += OnStartClicked;
            menuView.BuyAdsClicked += OnBuyAdsClicked;
            menuView.ExitGameClicked += OnExitGameClicked;
            
            purchaseService.NoAdsPurchased += menuView.DisableBuyButton;
            
            if (purchaseService.GetPurchasedData().noAds)
            {
                menuView.DisableBuyButton();
            }
        }

        private void OnStartClicked()
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            int nextIndex = currentIndex + 1;
            if (nextIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadSceneAsync(nextIndex, LoadSceneMode.Single);
            }
            else
            {
                Debug.LogWarning("No next scene in build settings.");
            }
        }

        private void OnBuyAdsClicked()
        {
            purchaseService.InitiatePurchase(noAdsProductId);
        }

        private void OnExitGameClicked()
        {
            Application.Quit();
        }

        public void Dispose()
        {
            menuView.StartClicked -= OnStartClicked;
            menuView.BuyAdsClicked -= OnBuyAdsClicked;
            menuView.ExitGameClicked -= OnExitGameClicked;
            
            purchaseService.NoAdsPurchased -= menuView.DisableBuyButton;
        }
    }
}