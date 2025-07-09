using System;
using Config;
using UniRx;
using UnityEngine;
using UnityEngine.Advertisements;
using Zenject;

namespace GameAdvertisement
{
    public enum AdPlace
    {
        Unknown,
        GameOver
    }
    
    public class UnityAdsService : IAdService, IInitializable
    { 
        private const string androidInterAdUnitId = "Interstitial_Android";
        private const string iOSInterAdUnitId = "Interstitial_iOS";
        private const string androidRewardAdUnitId = "Rewarded_Android";
        private const string iOSRewardAdUnitId = "Rewarded_iOS";
        
        private string adInterUnitId;
        private string adRewardUnitId;

        private string androidGameId;
        private string iOSGameId;
        private bool testMode;
        private string gameId;
        private AdPlace currentAdPlace = AdPlace.Unknown;
        
        private readonly Subject<AdPlace> onRewardedAdShowCompleted = new ();
        private readonly Subject<AdPlace> onRewardedAdShowFailed = new ();
        private readonly Subject<AdPlace> onInterstitialAdShowCompleted = new ();
        private readonly Subject<AdPlace> onInterstitialAdShowFailed = new ();
        public IObservable<AdPlace> OnRewardedAdShowCompleted => onRewardedAdShowCompleted;
        public IObservable<AdPlace> OnRewardedAdShowFailed => onRewardedAdShowFailed;
        public IObservable<AdPlace> OnInterstitialAdShowCompleted => onInterstitialAdShowCompleted;
        public IObservable<AdPlace> OnInterstitialAdShowFailed => onInterstitialAdShowFailed;

        [Inject]
        private void Construct(GameConfigurationSO gameConfigurationSO)
        {
            androidGameId = gameConfigurationSO.androidGameId;
            iOSGameId = gameConfigurationSO.iOSGameId;
            testMode = gameConfigurationSO.adTestMode;
        }
        
        public void Initialize()
        {
#if UNITY_IOS
            gameId = iOSGameId;
#elif UNITY_ANDROID
            gameId = androidGameId;
#elif UNITY_EDITOR
            gameId = androidGameId;
#endif
 
            if (!Advertisement.isInitialized && Advertisement.isSupported)
            {
                Advertisement.Initialize(gameId, testMode, this);
            }
            
            adInterUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? iOSInterAdUnitId
                : androidInterAdUnitId;
            
            adRewardUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? iOSRewardAdUnitId
                : androidRewardAdUnitId;
            
            Advertisement.Load(adInterUnitId, this);
            Advertisement.Load(adRewardUnitId, this);
        }

        public void ShowInterstitialAd(AdPlace place)
        {
            currentAdPlace = place;
            Advertisement.Show(adInterUnitId, this as IUnityAdsShowListener);
        }

        public void ShowRewardedAd(AdPlace place)
        {
            currentAdPlace = place;
            Advertisement.Show(adRewardUnitId, this as IUnityAdsShowListener);
        }

        public void OnInitializationComplete()
        {
            Debug.Log("done initializing Unity Ads");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            if (placementId.Equals(adRewardUnitId))
                Debug.Log("Rewarded ad loaded successfully.");
            else if (placementId.Equals(adInterUnitId))
                Debug.Log("Interstitial ad loaded successfully.");
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit {placementId}: {error.ToString()} - {message}");
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            if (placementId.Equals(adRewardUnitId))
                onRewardedAdShowFailed.OnNext(currentAdPlace);
            else if (placementId.Equals(adInterUnitId))
                onInterstitialAdShowFailed.OnNext(currentAdPlace);
            
            Debug.Log($"Error showing Ad Unit {placementId}: {error.ToString()} - {message}");
        }

        public void OnUnityAdsShowStart(string placementId)
        {
            Debug.Log($"Ad Unit {placementId} started showing.");
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            Debug.Log($"Ad Unit {placementId} clicked.");
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (placementId.Equals(adRewardUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
                onRewardedAdShowCompleted.OnNext(currentAdPlace);
            else if (placementId.Equals(adInterUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
                onInterstitialAdShowCompleted.OnNext(currentAdPlace);
        }
    }
}