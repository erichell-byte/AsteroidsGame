using TMPro;
using UniRx;
using UnityEngine;

namespace UI
{
	public class GameUIView : MonoBehaviour
	{
		[SerializeField] private TMP_Text _coordinateText;
		[SerializeField] private TMP_Text _rotationAngleText;
		[SerializeField] private TMP_Text _currentSpeedText;
		[SerializeField] private TMP_Text _countOfAvailableLaserText;
		[SerializeField] private TMP_Text _timeToResetLaserText;
		[SerializeField] private ButtonView _startGameButton;
		[SerializeField] private ButtonView _finishGameButton;

		private CompositeDisposable _disposables = new();

		private GameUIViewModel _gameUIViewModel;

		public void Initialize(GameUIViewModel gameUIViewModel)
		{
			_gameUIViewModel = gameUIViewModel;
			_disposables = new CompositeDisposable();

			gameUIViewModel.Coordinate.Subscribe(SetCoordinate).AddTo(_disposables);
			gameUIViewModel.RotationAngle.Subscribe(SetRotationAngle).AddTo(_disposables);
			gameUIViewModel.CurrentSpeed.Subscribe(SetCurrentSpeed).AddTo(_disposables);
			gameUIViewModel.LaserCount.Subscribe(SetCountOfAvailableLaser).AddTo(_disposables);
			gameUIViewModel.TimeToResetLaser.Subscribe(SetTimeToResetLaser).AddTo(_disposables);
			gameUIViewModel.GameStartedButtonEnabled.Subscribe(SetStartGameButtonActive).AddTo(_disposables);

			_startGameButton.OnClick += _gameUIViewModel.StartGameButtonClicked;
			_finishGameButton.OnClick += FinishGameButtonClicked;
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}

		public void Show()
		{
			gameObject.SetActive(true);
		}

		private void FinishGameButtonClicked()
		{
			Hide();
			_gameUIViewModel.FinishGameButtonClicked();
		}

		public void SetCoordinate(Vector2 coordinate)
		{
			_coordinateText.text = $"x: {coordinate.x:F2} , y:{coordinate.y:F2}";
		}

		public void SetRotationAngle(float rotationAngle)
		{
			_rotationAngleText.text = $"angle z: {rotationAngle:F2}";
		}

		public void SetCurrentSpeed(float speed)
		{
			_currentSpeedText.text = $"speed: {speed:F2}";
		}

		public void SetCountOfAvailableLaser(int countOfAvailableLaser)
		{
			_countOfAvailableLaserText.text = $"laserShots: {countOfAvailableLaser}";
		}

		public void SetTimeToResetLaser(float timeToResetLaser)
		{
			_timeToResetLaserText.text = $"time to recovery laser shots: {timeToResetLaser:F2}";
		}

		public void SetStartGameButtonActive(bool isActive)
		{
			if (_startGameButton == null) return;
			var go = _startGameButton.gameObject;
			if (!go) return;
			go.SetActive(isActive);
		}

		private void Dispose()
		{
			if (_startGameButton != null) _startGameButton.OnClick -= _gameUIViewModel.StartGameButtonClicked;
			if (_finishGameButton != null) _finishGameButton.OnClick -= _gameUIViewModel.FinishGameButtonClicked;
			_disposables?.Clear();
			_gameUIViewModel = null;
		}

		private void OnDestroy()
		{
			Dispose();
		}
	}
}