using Config;
using UniRx;
using UnityEngine;
using UnityEngine.Advertisements;
using Zenject;

namespace GameAdvertisement
{
    public class UnityAdController : IAdController
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
        
        public ReactiveCommand OnRewardedAdShowCompleted { get; } = new();
        public ReactiveCommand OnRewardedAdShowFailed { get; } = new();
        public ReactiveCommand OnInterstitialAdShowCompleted { get; } = new();
        public ReactiveCommand OnInterstitialAdShowFailed { get; } = new();

        [Inject]
        private void Construct(GameConfiguration gameConfiguration)
        {
            androidGameId = gameConfiguration.androidGameId;
            iOSGameId = gameConfiguration.iOSGameId;
            testMode = gameConfiguration.adTestMode;
        }
        
        public void Initialize()
        {
            ((IAdController)this).AdInit();
            Advertisement.Load(adInterUnitId, this);
            Advertisement.Load(adRewardUnitId, this);
        }
        

        void IAdController.AdInit()
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
        }

        public void ShowInterstitialAd()
        {
            Advertisement.Show(adInterUnitId, this as IUnityAdsShowListener);
        }

        public void ShowRewardedAd()
        {
            Advertisement.Show(adRewardUnitId, this as IUnityAdsShowListener);
        }

        public void OnInitializationComplete()
        {
            
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            
        }
        
        public void OnUnityAdsAdLoaded(string placementId)
        {
            
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit {placementId}: {error.ToString()} - {message}");
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            if (placementId.Equals(adRewardUnitId))
                OnRewardedAdShowFailed.Execute();
            else if (placementId.Equals(adInterUnitId))
                OnInterstitialAdShowFailed.Execute();
            
            Debug.Log($"Error showing Ad Unit {placementId}: {error.ToString()} - {message}");
        }

        public void OnUnityAdsShowStart(string placementId)
        {
           
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (placementId.Equals(adRewardUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
                OnRewardedAdShowCompleted.Execute();
            else if (placementId.Equals(adInterUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
                OnInterstitialAdShowCompleted.Execute();
        }
    }
}