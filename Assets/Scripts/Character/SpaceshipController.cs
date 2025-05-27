using System;
using Components;
using Config;
using Enemies;
using Systems;
using UnityEngine;
using Zenject;

namespace Character
{
    public class SpaceshipController :
        MonoBehaviour,
        IGameStartListener,
        IGameFinishListener
    {
        private GameConfiguration config;
        
        [SerializeField] private KeyboardInputReceiver inputReceiver;
        [SerializeField] private Transform poolParent;

        private MoveComponent moveComponent;
        private AttackComponent attackComponent;

        [Inject]
        private void Construct(GameConfiguration config)
        {
            this.config = config;
        }
        
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