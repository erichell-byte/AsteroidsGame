using System.Collections;
using Components;
using Config;
using Systems;
using UnityEngine;

namespace UI
{
    public class ShipDataObserver : MonoBehaviour, IGameStartListener, IGameFinishListener
    {
        [SerializeField] private AttackComponent attackComponent;
        [SerializeField] private MoveComponent moveComponent;
        [SerializeField] private ShipDataView shipDataView;
        [SerializeField] private GameConfiguration config;

        public void OnStartGame()
        {
            Initialize();
            StartCoroutine(CreateEventSubscription());
        }

        public void OnFinishGame()
        {
            DestroyEventSubscription();
        }

        private IEnumerator CreateEventSubscription()
        {
            yield return new WaitForSeconds(0.5f);
            moveComponent.OnCoordinateChanged += shipDataView.SetCoordinate;
            moveComponent.OnRotationChanged += shipDataView.SetRotationAngle;
            moveComponent.OnSpeedChanged += shipDataView.SetCurrentSpeed;
            attackComponent.GetLaserWeapon().OnLaserCountChanged += shipDataView.SetCountOfAvailableLaser;
            attackComponent.GetLaserWeapon().OnTimeToRecoveryChanged += shipDataView.SetTimeToResetLaser;
        }

        private void DestroyEventSubscription()
        {
            moveComponent.OnCoordinateChanged -= shipDataView.SetCoordinate;
            moveComponent.OnRotationChanged -= shipDataView.SetRotationAngle;
            moveComponent.OnSpeedChanged -= shipDataView.SetCurrentSpeed;
            attackComponent.GetLaserWeapon().OnLaserCountChanged -= shipDataView.SetCountOfAvailableLaser;
            attackComponent.GetLaserWeapon().OnTimeToRecoveryChanged -= shipDataView.SetTimeToResetLaser;
        }

        private void Initialize()
        {
            shipDataView.SetCoordinate(moveComponent.transform.position);
            shipDataView.SetRotationAngle(0);
            shipDataView.SetCurrentSpeed(0f);
            shipDataView.SetCountOfAvailableLaser(config.countOfLaserShots);
            shipDataView.SetTimeToResetLaser(0f);
        }
    }
}