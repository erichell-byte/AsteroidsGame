using GameAdvertisement;
using UniRx;

namespace Project.Scripts.UI
{
    public class AdViewModel
    {
        private IAdController adController;

        public ReactiveCommand<Unit> ShowRewardedButtonClickedCommand { get; } = new();
        public ReactiveCommand<Unit> SkipRewardedButtonClickedCommand { get; } = new();

        public AdViewModel(IAdController adController)
        {
            this.adController = adController;
        }

        public void ShowRewardedButtonClicked()
        {
            adController.ShowRewardedAd();
        }

        public void SkipRewardedButtonClicked()
        {
            adController.ShowInterstitialAd();
        }
    }
}