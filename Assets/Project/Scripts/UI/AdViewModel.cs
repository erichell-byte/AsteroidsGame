using GameAdvertisement;
using UniRx;

namespace Project.Scripts.UI
{
    public class AdViewModel
    {
        private IAdService adService;

        public AdViewModel(IAdService adService)
        {
            this.adService = adService;
        }

        public void ShowRewardedButtonClicked()
        {
            adService.ShowRewardedAd(AdPlace.GameOver);
        }

        public void SkipRewardedButtonClicked()
        {
            adService.ShowInterstitialAd(AdPlace.GameOver);
        }
    }
}