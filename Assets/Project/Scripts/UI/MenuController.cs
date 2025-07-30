using System;
using Config;
using Purchasing;
using Systems;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UI
{
    public class MenuController: IDisposable
    {
        private MenuView _menuView;
        private IPurchaseService _purchaseService;
        private ISceneLoader _sceneLoader;
        private string _noAdsProductId;

        [Inject]
        private void Construct(
            MenuView menuView,
            IPurchaseService purchaseService,
            GameConfiguration gameConfig,
            ISceneLoader sceneLoader)
        {
            _menuView = menuView;
            _purchaseService = purchaseService;
            _sceneLoader = sceneLoader;

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
            _sceneLoader.LoadNextScene();
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