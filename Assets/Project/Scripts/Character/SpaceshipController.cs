using System;
using Components;
using Config;
using Input;
using Systems;
using UniRx;
using UnityEngine;
using Zenject;

namespace Character
{
    public class SpaceshipController :
        IGameStartListener,
        IGamePauseListener,
        IGameResumeListener,
        IGameFinishListener,
        ITickable
    {
        private GameConfigurationSO config;
        private MoveComponent moveComponent;
        private AttackComponent attackComponent;
        private SpaceshipModel spaceshipModel;
        private KeyboardInputReceiver inputReceiver;
        
        public SpaceshipModel SpaceshipModel => spaceshipModel;
 
        [Inject]
        private void Construct(
            GameConfigurationSO config,
            MoveComponent moveComponent,
            AttackComponent attackComponent,
            GameCycle gameCycle,
            SpaceshipModel spaceshipModel)
        {
            this.config = config;
            this.moveComponent = moveComponent;
            this.attackComponent = attackComponent;
            this.spaceshipModel = spaceshipModel;
            inputReceiver = new KeyboardInputReceiver();
            
            gameCycle.AddListener(this);
            moveComponent.SetInitialPositionAndRotation(spaceshipModel.Position.Value, spaceshipModel.Rotation.Value);
        }
        
        private void ResetCharacterModel()
        {
            spaceshipModel.SetPosition(Vector3.zero);
            spaceshipModel.SetRotation(0f);
            spaceshipModel.SetSpeed(0f);
            spaceshipModel.SetLaserCount(config.remoteConfig.countOfLaserShots);
            spaceshipModel.SetTimeToRecoveryLaser(0f);
            spaceshipModel.SetIsDead(true);
        }

        public void OnStartGame()
        {
            moveComponent.Initialize(spaceshipModel);
            attackComponent.Initialize(spaceshipModel);

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
            ResetCharacterModel();
        }

        public void Tick()
        {
            inputReceiver.Tick();
        }

        public void OnPauseGame()
        {
            inputReceiver.InputMoveValue -= moveComponent.MoveForward;
            inputReceiver.InputRotationValue -= moveComponent.Rotate;

            inputReceiver.InputMainShotValue -= attackComponent.AttackByMainShot;
            inputReceiver.InputAdditionalShotValue -= attackComponent.AttackByLaserShot;
        }

        public void OnResumeGame()
        {
            moveComponent.Initialize(spaceshipModel);
            attackComponent.Initialize(spaceshipModel);

            inputReceiver.InputMoveValue += moveComponent.MoveForward;
            inputReceiver.InputRotationValue += moveComponent.Rotate;

            inputReceiver.InputMainShotValue += attackComponent.AttackByMainShot;
            inputReceiver.InputAdditionalShotValue += attackComponent.AttackByLaserShot;
        }
    }
}