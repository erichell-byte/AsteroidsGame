using GameAdvertisement;
using Purchasing;

namespace UI
{
	public class GameOverViewModel
	{
		private readonly IAdService _adService;
		private readonly IPurchaseService _purchaseService;

		public GameOverViewModel(
			IAdService adService,
			IPurchaseService purchaseService)
		{
			_adService = adService;
			_purchaseService = purchaseService;
		}

		private bool IsNoAdsPurchased => _purchaseService.GetPurchasedData().NoAds;

		public void ShowRewardedButtonClicked()
		{
			_adService.ShowRewardedAd(AdPlace.GameOver);
		}

		public void SkipRewardedButtonClicked()
		{
			if (IsNoAdsPurchased)
				_adService.SkipInterstitial();
			else
				_adService.ShowInterstitialAd(AdPlace.GameOver);
		}
	}
}