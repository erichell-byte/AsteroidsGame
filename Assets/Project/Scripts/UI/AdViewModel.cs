using GameAdvertisement;
using Purchasing;

namespace UI
{
    public class AdViewModel
    {
        private readonly IAdService _adService;
        private readonly IPurchaseService _purchaseService;
        private bool IsNoAdsPurchased => _purchaseService.GetPurchasedData().NoAds;

        public AdViewModel(
            IAdService adService,
            IPurchaseService purchaseService)
        {
            _adService = adService;
            _purchaseService = purchaseService;
        }

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