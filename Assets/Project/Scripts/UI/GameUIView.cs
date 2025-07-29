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
        
        private readonly CompositeDisposable _disposables = new();

        private GameUIViewModel _gameUIViewModel;

        public void Initialize(GameUIViewModel gameUIViewModel)
        {
            _gameUIViewModel = gameUIViewModel;

            gameUIViewModel.Coordinate.Subscribe(SetCoordinate).AddTo(_disposables);
            gameUIViewModel.RotationAngle.Subscribe(SetRotationAngle).AddTo(_disposables);
            gameUIViewModel.CurrentSpeed.Subscribe(SetCurrentSpeed).AddTo(_disposables);
            gameUIViewModel.LaserCount.Subscribe(SetCountOfAvailableLaser).AddTo(_disposables);
            gameUIViewModel.TimeToResetLaser.Subscribe(SetTimeToResetLaser).AddTo(_disposables);
            gameUIViewModel.GameStartedButtonEnabled.Subscribe(SetStartGameButtonActive).AddTo(_disposables);
        
            _startGameButton.OnClick += gameUIViewModel.StartGameButtonClicked;
        }
    
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    
        public void Show()
        {
            gameObject.SetActive(true);
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
            _startGameButton.gameObject.SetActive(isActive);
        }
    
        public void Dispose()
        {
            if (_gameUIViewModel == null) return;
            _startGameButton.OnClick -= _gameUIViewModel.StartGameButtonClicked;
            _disposables?.Clear();
        }
    }
}