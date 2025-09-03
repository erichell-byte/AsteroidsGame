using DG.Tweening;
using UnityEngine;

namespace UI
{
	public class GameOverView : MonoBehaviour
	{
		[SerializeField] private ButtonView _showRewardedAdButton;
		[SerializeField] private ButtonView _skipRewardedAdButton;
		[SerializeField] private Transform _gameOverWindow;

		private GameOverViewModel _gameOverViewModel;
		private RectTransform _gameOverRT;

		public void Initialize(GameOverViewModel gameOverViewModel)
		{
			_gameOverRT = _gameOverWindow.GetComponent<RectTransform>();

			_gameOverViewModel = gameOverViewModel;
			_showRewardedAdButton.OnClick += gameOverViewModel.ShowRewardedButtonClicked;
			_skipRewardedAdButton.OnClick += gameOverViewModel.SkipRewardedButtonClicked;
		}

		public void Show()
		{
			gameObject.SetActive(true);
			_gameOverWindow.DOKill(true);
			_gameOverWindow.localScale = Vector3.one * 0.7f;
			_gameOverWindow.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
		}

		public void Hide()
		{
			var startPos = _gameOverWindow.localPosition;
			DOTween.Sequence()
				.Append(_gameOverWindow.DOScale(0.7f, 0.2f).SetEase(Ease.InQuad))
				.Join(_gameOverRT.DOAnchorPosY(_gameOverRT.anchoredPosition.y + 500, 0.2f).SetEase(Ease.InQuad))
				.OnComplete(() =>
				{
					gameObject.SetActive(false);
					_gameOverWindow.localPosition = startPos;
				});
		}

		public void Dispose()
		{
			_showRewardedAdButton.OnClick -= _gameOverViewModel.ShowRewardedButtonClicked;
			_skipRewardedAdButton.OnClick -= _gameOverViewModel.SkipRewardedButtonClicked;
		}
	}
}