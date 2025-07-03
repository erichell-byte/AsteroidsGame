using System;
using UnityEngine.Advertisements;

namespace GameAdvertisement
{
    public interface IAdService : IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        public IObservable<AdPlace> OnRewardedAdShowCompleted { get; }
        public IObservable<AdPlace> OnRewardedAdShowFailed { get; }
        
        public IObservable<AdPlace> OnInterstitialAdShowCompleted { get; }
        public IObservable<AdPlace> OnInterstitialAdShowFailed { get; }
        
        public void ShowInterstitialAd(AdPlace place);
        
        public void ShowRewardedAd(AdPlace place);

    }
}