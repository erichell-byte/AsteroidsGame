using GameAdvertisement;
using Project.Scripts.Purchasing;

namespace Project.Scripts.UI
{
    public class AdViewModel
    {
        private IAdService adService;
        private IPurchaseService purchaseService;
        private bool isNoAdsPurchased => purchaseService.GetPurchasedData().noAds;

        public AdViewModel(
            IAdService adService,
            IPurchaseService purchaseService)
        {
            this.adService = adService;
            this.purchaseService = purchaseService;
        }

        public void ShowRewardedButtonClicked()
        {
            adService.ShowRewardedAd(AdPlace.GameOver);
        }

        public void SkipRewardedButtonClicked()
        {
            if (isNoAdsPurchased)
                adService.SkipInterstitial();
            else
                adService.ShowInterstitialAd(AdPlace.GameOver);
        }
    }
}