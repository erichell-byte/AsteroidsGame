using System;
using UniRx;
using UnityEngine.Advertisements;

namespace GameAdvertisement
{
	public interface IAdService : IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
	{
		public IObservable<AdPlace> OnRewardedAdShowCompleted { get; }
		public IObservable<AdPlace> OnRewardedAdShowFailed { get; }

		public IObservable<AdPlace> OnInterstitialAdShowCompleted { get; }
		public IObservable<AdPlace> OnInterstitialAdShowFailed { get; }
		public IObservable<Unit> OnSkipInterstitialAdBecauseNoAdsPurchased { get; }

		public void ShowInterstitialAd(AdPlace place);

		public void ShowRewardedAd(AdPlace place);

		public void SkipInterstitial();
	}
}