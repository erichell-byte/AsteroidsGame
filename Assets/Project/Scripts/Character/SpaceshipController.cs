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
        private RemoteConfig _remoteConfig;
        private MoveComponent _moveComponent;
        private AttackComponent _attackComponent;
        private SpaceshipModel _spaceshipModel;
        private KeyboardInputReceiver _inputReceiver;
        
        public SpaceshipModel SpaceshipModel => _spaceshipModel;
 
        [Inject]
        private void Construct(
            IConfigProvider configProvider,
            MoveComponent moveComponent,
            AttackComponent attackComponent,
            GameCycle gameCycle,
            SpaceshipModel spaceshipModel)
        {
            _remoteConfig = configProvider.GetRemoteConfig();
            _moveComponent = moveComponent;
            _attackComponent = attackComponent;
            _spaceshipModel = spaceshipModel;
            _inputReceiver = new KeyboardInputReceiver();
            
            gameCycle.AddListener(this);
            moveComponent.SetInitialPositionAndRotation(spaceshipModel.Position.Value, spaceshipModel.Rotation.Value);
        }
        
        private void ResetCharacterModel()
        {
            _spaceshipModel.SetPosition(Vector3.zero);
            _spaceshipModel.SetRotation(0f);
            _spaceshipModel.SetSpeed(0f);
            _spaceshipModel.SetLaserCount(_remoteConfig.CountOfLaserShots);
            _spaceshipModel.SetTimeToRecoveryLaser(0f);
            _spaceshipModel.SetIsDead(true);
        }

        public void OnStartGame()
        {
            _moveComponent.Initialize(_spaceshipModel);
            _attackComponent.Initialize(_spaceshipModel);

            _inputReceiver.InputMoveValue += _moveComponent.MoveForward;
            _inputReceiver.InputRotationValue += _moveComponent.Rotate;

            _inputReceiver.InputMainShotValue += _attackComponent.AttackByMainShot;
            _inputReceiver.InputAdditionalShotValue += _attackComponent.AttackByLaserShot;
        }
        
        public void OnFinishGame()
        {
            _inputReceiver.InputMoveValue -= _moveComponent.MoveForward;
            _inputReceiver.InputRotationValue -= _moveComponent.Rotate;

            _inputReceiver.InputMainShotValue -= _attackComponent.AttackByMainShot;
            _inputReceiver.InputAdditionalShotValue -= _attackComponent.AttackByLaserShot;

            _attackComponent.ResetWeapon();
            ResetCharacterModel();
        }

        public void Tick()
        {
            _inputReceiver.Tick();
        }

        public void OnPauseGame()
        {
            _inputReceiver.InputMoveValue -= _moveComponent.MoveForward;
            _inputReceiver.InputRotationValue -= _moveComponent.Rotate;

            _inputReceiver.InputMainShotValue -= _attackComponent.AttackByMainShot;
            _inputReceiver.InputAdditionalShotValue -= _attackComponent.AttackByLaserShot;
        }

        public void OnResumeGame()
        {
            _moveComponent.Initialize(_spaceshipModel);
            _attackComponent.Initialize(_spaceshipModel);

            _inputReceiver.InputMoveValue += _moveComponent.MoveForward;
            _inputReceiver.InputRotationValue += _moveComponent.Rotate;

            _inputReceiver.InputMainShotValue += _attackComponent.AttackByMainShot;
            _inputReceiver.InputAdditionalShotValue += _attackComponent.AttackByLaserShot;
        }
    }
}