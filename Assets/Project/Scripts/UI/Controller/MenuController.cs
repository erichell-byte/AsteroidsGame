using System;
using AssetsLoader;
using Config;
using Cysharp.Threading.Tasks;
using Purchasing;
using Systems;
using UnityEngine;
using Zenject;

namespace UI
{
	public class MenuController : IDisposable
	{
		private IAssetsPreloader _assetsPreloader;
		private GameConfiguration _gameConfig;
		private MenuView _menuView;
		private string _noAdsProductId;
		private IPurchaseService _purchaseService;
		private ISceneLoader _sceneLoader;

		[Inject]
		private void Construct(
			MenuView menuView,
			IPurchaseService purchaseService,
			GameConfiguration gameConfig,
			ISceneLoader sceneLoader,
			IAssetsPreloader assetsPreloader)
		{
			_menuView = menuView;
			_purchaseService = purchaseService;
			_sceneLoader = sceneLoader;
			_assetsPreloader = assetsPreloader;
			_gameConfig = gameConfig;

			_noAdsProductId = gameConfig.NoAdsProductId;

			menuView.StartClicked += OnStartClicked;
			menuView.BuyAdsClicked += OnBuyAdsClicked;
			menuView.ExitGameClicked += OnExitGameClicked;

			purchaseService.OnPurchasedAction += menuView.DisableBuyButton;

			if (purchaseService.GetPurchasedData().NoAds) menuView.DisableBuyButton(TypeOfPurchase.NoAds);

			InitializeAsync().Forget();
		}

		public void Dispose()
		{
			_menuView.StartClicked -= OnStartClicked;
			_menuView.BuyAdsClicked -= OnBuyAdsClicked;
			_menuView.ExitGameClicked -= OnExitGameClicked;

			_purchaseService.OnPurchasedAction -= _menuView.DisableBuyButton;
		}

		private async UniTaskVoid InitializeAsync()
		{
			if (_menuView == null) return;
			_menuView.SetStartButtonInteractable(false);
			var assets = new[]
			{
				_gameConfig.BulletId,
				_gameConfig.AsteroidId,
				_gameConfig.AsteroidSmallId,
				_gameConfig.UfoId,
				_gameConfig.Effects.ShotVfx,
				_gameConfig.Effects.AsteroidExplosionVfx,
				_gameConfig.Effects.UfoExplosionVfx
			};
			await _assetsPreloader.PreloadAsync(assets);
			_menuView.SetStartButtonInteractable(true);
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
	}
}