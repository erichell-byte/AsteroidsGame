using System;
using UniRx;
using UnityEngine.Advertisements;
using Zenject;

namespace GameAdvertisement
{
    public interface IAdController : IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener, IInitializable
    {
        public ReactiveCommand OnRewardedAdShowCompleted { get; }
        public ReactiveCommand OnRewardedAdShowFailed { get; }
        
        public ReactiveCommand OnInterstitialAdShowCompleted { get; }
        public ReactiveCommand OnInterstitialAdShowFailed { get; }
        
        protected internal void AdInit();
        
        public void ShowInterstitialAd();
        
        public void ShowRewardedAd();

    }
}