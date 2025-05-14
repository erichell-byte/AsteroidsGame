using System;
using Components;
using Config;
using Enemies;
using Systems;
using UnityEngine;

namespace Character
{
    public class SpaceshipController :
        MonoBehaviour,
        IGameStartListener,
        IGameFinishListener
    {
        [SerializeField] private KeyboardInputReceiver inputReceiver;
        [SerializeField] private GameConfiguration config;
        [SerializeField] private Transform poolParent;

        private MoveComponent moveComponent;
        private AttackComponent attackComponent;

        public event Action OnShipDie;

        public void OnStartGame()
        {
            moveComponent = GetComponent<MoveComponent>();
            attackComponent = GetComponent<AttackComponent>();

            moveComponent.Initialize(
                config.moveCoefficient,
                config.rotateCoefficient,
                config.maxVelocityMagnitude);

            attackComponent.Initialize(
                config.shotFrequency,
                config.timeToRecoveryLaser,
                config.timeToDurationLaser,
                config.countOfLaserShots,
                config.bulletSpeed,
                config.bulletPrefab,
                poolParent);

            inputReceiver.InputMoveValue += moveComponent.MoveForward;
            inputReceiver.InputRotationValue += moveComponent.Rotate;

            inputReceiver.InputMainShotValue += attackComponent.AttackByMainShot;
            inputReceiver.InputAdditionalShotValue += attackComponent.AttackByLaserShot;
        }

        public void OnFinishGame()
        {
            inputReceiver.InputMoveValue -= moveComponent.MoveForward;
            inputReceiver.InputRotationValue -= moveComponent.Rotate;

            inputReceiver.InputMainShotValue -= attackComponent.AttackByMainShot;
            inputReceiver.InputAdditionalShotValue -= attackComponent.AttackByLaserShot;

            attackComponent.ResetWeapon();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Enemy>())
            {
                OnShipDie?.Invoke();
            }
        }
    }
}