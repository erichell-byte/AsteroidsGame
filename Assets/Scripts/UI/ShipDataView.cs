using TMPro;
using UnityEngine;

public class ShipDataView : MonoBehaviour
{
    [SerializeField] private TMP_Text coordinateText;
    [SerializeField] private TMP_Text rotationAngleText;
    [SerializeField] private TMP_Text currentSpeedText;
    [SerializeField] private TMP_Text countOfAvailableLaserText;
    [SerializeField] private TMP_Text timeToResetLaserText;

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
}