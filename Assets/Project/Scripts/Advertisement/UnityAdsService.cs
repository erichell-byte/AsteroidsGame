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
		private const string AndroidInterAdUnitId = "Interstitial_Android";
		private const string IOSInterAdUnitId = "Interstitial_iOS";
		private const string AndroidRewardAdUnitId = "Rewarded_Android";
		private const string IOSRewardAdUnitId = "Rewarded_iOS";
		private readonly Subject<AdPlace> _onInterstitialAdShowCompleted = new();
		private readonly Subject<AdPlace> _onInterstitialAdShowFailed = new();

		private readonly Subject<AdPlace> _onRewardedAdShowCompleted = new();
		private readonly Subject<AdPlace> _onRewardedAdShowFailed = new();
		private readonly Subject<Unit> _onSkipInterstitialNoAdsPurchased = new();

		private string _adInterUnitId;
		private string _adRewardUnitId;

		private string _androidGameId;
		private AdPlace _currentAdPlace = AdPlace.Unknown;
		private string _gameId;
		private string _iOSGameId;
		private bool _testMode;
		public IObservable<AdPlace> OnRewardedAdShowCompleted => _onRewardedAdShowCompleted;
		public IObservable<AdPlace> OnRewardedAdShowFailed => _onRewardedAdShowFailed;
		public IObservable<AdPlace> OnInterstitialAdShowCompleted => _onInterstitialAdShowCompleted;
		public IObservable<AdPlace> OnInterstitialAdShowFailed => _onInterstitialAdShowFailed;
		public IObservable<Unit> OnSkipInterstitialAdBecauseNoAdsPurchased => _onSkipInterstitialNoAdsPurchased;

		[Inject]
		private void Construct(GameConfiguration gameConfiguration)
		{
			_androidGameId = gameConfiguration.AndroidGameId;
			_iOSGameId = gameConfiguration.IOSGameId;
			_testMode = gameConfiguration.AdTestMode;
		}

		public void ShowInterstitialAd(AdPlace place)
		{
			_currentAdPlace = place;
			Advertisement.Show(_adInterUnitId, this);
		}

		public void ShowRewardedAd(AdPlace place)
		{
			_currentAdPlace = place;
			Advertisement.Show(_adRewardUnitId, this);
		}

		public void SkipInterstitial()
		{
			_onSkipInterstitialNoAdsPurchased.OnNext(Unit.Default);
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
			if (placementId.Equals(_adRewardUnitId))
				Debug.Log("Rewarded ad loaded successfully.");
			else if (placementId.Equals(_adInterUnitId))
				Debug.Log("Interstitial ad loaded successfully.");
		}

		public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
		{
			Debug.Log($"Error loading Ad Unit {placementId}: {error.ToString()} - {message}");
		}

		public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
		{
			if (placementId.Equals(_adRewardUnitId))
				_onRewardedAdShowFailed.OnNext(_currentAdPlace);
			else if (placementId.Equals(_adInterUnitId))
				_onInterstitialAdShowFailed.OnNext(_currentAdPlace);

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
			if (placementId.Equals(_adRewardUnitId) &&
			    showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
				_onRewardedAdShowCompleted.OnNext(_currentAdPlace);
			else if (placementId.Equals(_adInterUnitId) &&
			         showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
				_onInterstitialAdShowCompleted.OnNext(_currentAdPlace);
		}

		public void Initialize()
		{
#if UNITY_IOS
            _gameId = _iOSGameId;
#elif UNITY_ANDROID
            _gameId = _androidGameId;
#elif UNITY_EDITOR
			_gameId = _androidGameId;
#endif

			if (!Advertisement.isInitialized && Advertisement.isSupported)
				Advertisement.Initialize(_gameId, _testMode, this);

			_adInterUnitId = Application.platform == RuntimePlatform.IPhonePlayer
				? IOSInterAdUnitId
				: AndroidInterAdUnitId;

			_adRewardUnitId = Application.platform == RuntimePlatform.IPhonePlayer
				? IOSRewardAdUnitId
				: AndroidRewardAdUnitId;

			Advertisement.Load(_adInterUnitId, this);
			Advertisement.Load(_adRewardUnitId, this);
		}
	}
}