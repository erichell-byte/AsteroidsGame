using System;
using Config;
using Purchasing;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UI
{
    public class MenuController: IDisposable
    {
        private MenuView _menuView;
        private IPurchaseService _purchaseService;
        private string _noAdsProductId;

        [Inject]
        private void Construct(
            MenuView menuView,
            IPurchaseService purchaseService,
            GameConfiguration gameConfig)
        {
            _menuView = menuView;
            _purchaseService = purchaseService;

            _noAdsProductId = gameConfig.NoAdsProductId;
            
            menuView.StartClicked += OnStartClicked;
            menuView.BuyAdsClicked += OnBuyAdsClicked;
            menuView.ExitGameClicked += OnExitGameClicked;
            
            purchaseService.OnPurchasedAction += menuView.DisableBuyButton;
            
            if (purchaseService.GetPurchasedData().NoAds)
            {
                menuView.DisableBuyButton(TypeOfPurchase.NoAds);
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
            _purchaseService.InitiatePurchase(_noAdsProductId);
        }

        private void OnExitGameClicked()
        {
            Application.Quit();
        }

        public void Dispose()
        {
            _menuView.StartClicked -= OnStartClicked;
            _menuView.BuyAdsClicked -= OnBuyAdsClicked;
            _menuView.ExitGameClicked -= OnExitGameClicked;
            
            _purchaseService.OnPurchasedAction -= _menuView.DisableBuyButton;
        }
    }
}