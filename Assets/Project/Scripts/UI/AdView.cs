using UnityEngine;

namespace UI
{
	public class AdView : MonoBehaviour
	{
		[SerializeField] private ButtonView _showRewardedAdButton;
		[SerializeField] private ButtonView _skipRewardedAdButton;

		private AdViewModel _adViewModel;

		public void Initialize(AdViewModel adViewModel)
		{
			_adViewModel = adViewModel;
			_showRewardedAdButton.OnClick += adViewModel.ShowRewardedButtonClicked;
			_skipRewardedAdButton.OnClick += adViewModel.SkipRewardedButtonClicked;
		}

		public void Show()
		{
			gameObject.SetActive(true);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}

		public void Dispose()
		{
			_showRewardedAdButton.OnClick -= _adViewModel.ShowRewardedButtonClicked;
			_skipRewardedAdButton.OnClick -= _adViewModel.SkipRewardedButtonClicked;
		}
	}
}