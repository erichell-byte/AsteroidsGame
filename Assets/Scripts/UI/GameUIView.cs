using Character;
using TMPro;
using UniRx;
using UnityEngine;

public class GameUIView : MonoBehaviour
{
    [SerializeField] private TMP_Text coordinateText;
    [SerializeField] private TMP_Text rotationAngleText;
    [SerializeField] private TMP_Text currentSpeedText;
    [SerializeField] private TMP_Text countOfAvailableLaserText;
    [SerializeField] private TMP_Text timeToResetLaserText;
    [SerializeField] private ButtonView startGameButton;

    private GameUIViewModel gameUIViewModel;
    private CompositeDisposable disposables = new();

    public void Initialize(GameUIViewModel gameUIViewModel)
    {
        this.gameUIViewModel = gameUIViewModel;

        gameUIViewModel.Coordinate.Subscribe(SetCoordinate).AddTo(disposables);
        gameUIViewModel.RotationAngle.Subscribe(SetRotationAngle).AddTo(disposables);
        gameUIViewModel.CurrentSpeed.Subscribe(SetCurrentSpeed).AddTo(disposables);
        gameUIViewModel.LaserCount.Subscribe(SetCountOfAvailableLaser).AddTo(disposables);
        gameUIViewModel.TimeToResetLaser.Subscribe(SetTimeToResetLaser).AddTo(disposables);
        gameUIViewModel.GameStartedButtonEnabled.Subscribe(SetStartGameButtonActive).AddTo(disposables);
        
        startGameButton.OnClick += gameUIViewModel.StartGameButtonClicked;
    }
    
    public void SetCoordinate(Vector2 coordinate)
    {
        coordinateText.text = $"x: {coordinate.x:F2} , y:{coordinate.y:F2}";
    }

    public void SetRotationAngle(float rotationAngle)
    {
        rotationAngleText.text = $"angle z: {rotationAngle:F2}";
    }

    public void SetCurrentSpeed(float speed)
    {
        currentSpeedText.text = $"speed: {speed:F2}";
    }

    public void SetCountOfAvailableLaser(int countOfAvailableLaser)
    {
        countOfAvailableLaserText.text = $"laserShots: {countOfAvailableLaser}";
    }

    public void SetTimeToResetLaser(float timeToResetLaser)
    {
        timeToResetLaserText.text = $"time to recovery laser shots: {timeToResetLaser:F2}";
    }

    public void SetStartGameButtonActive(bool isActive)
    {
        startGameButton.gameObject.SetActive(isActive);
    }

    public void Dispose()
    {
        if (gameUIViewModel == null) return;
        startGameButton.OnClick -= gameUIViewModel.StartGameButtonClicked;
        disposables?.Clear();
    }
}
