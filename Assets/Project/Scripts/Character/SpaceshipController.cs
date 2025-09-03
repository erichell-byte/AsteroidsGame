using Components;
using Config;
using Input;
using Systems;
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
		private AttackComponent _attackComponent;
		private KeyboardInputReceiver _inputReceiver;
		private MoveComponent _moveComponent;
		private RemoteConfig _remoteConfig;

		public SpaceshipModel SpaceshipModel { get; private set; }

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
			SpaceshipModel = spaceshipModel;
			_inputReceiver = new KeyboardInputReceiver();

			gameCycle.AddListener(this);
			moveComponent.SetInitialPositionAndRotation(spaceshipModel.Position.Value, spaceshipModel.Rotation.Value);
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

		public void OnPauseGame()
		{
			_inputReceiver.InputMoveValue -= _moveComponent.MoveForward;
			_inputReceiver.InputRotationValue -= _moveComponent.Rotate;

			_inputReceiver.InputMainShotValue -= _attackComponent.AttackByMainShot;
			_inputReceiver.InputAdditionalShotValue -= _attackComponent.AttackByLaserShot;
		}

		public void OnResumeGame()
		{
			_moveComponent.Initialize(SpaceshipModel);
			_attackComponent.Initialize(SpaceshipModel);

			_inputReceiver.InputMoveValue += _moveComponent.MoveForward;
			_inputReceiver.InputRotationValue += _moveComponent.Rotate;

			_inputReceiver.InputMainShotValue += _attackComponent.AttackByMainShot;
			_inputReceiver.InputAdditionalShotValue += _attackComponent.AttackByLaserShot;
		}

		public void OnStartGame()
		{
			_moveComponent.Initialize(SpaceshipModel);
			_attackComponent.Initialize(SpaceshipModel);

			_inputReceiver.InputMoveValue += _moveComponent.MoveForward;
			_inputReceiver.InputRotationValue += _moveComponent.Rotate;

			_inputReceiver.InputMainShotValue += _attackComponent.AttackByMainShot;
			_inputReceiver.InputAdditionalShotValue += _attackComponent.AttackByLaserShot;
		}

		public void Tick()
		{
			_inputReceiver.Tick();
		}

		private void ResetCharacterModel()
		{
			SpaceshipModel.SetPosition(Vector3.zero);
			SpaceshipModel.SetRotation(0f);
			SpaceshipModel.SetSpeed(0f);
			SpaceshipModel.SetLaserCount(_remoteConfig.CountOfLaserShots);
			SpaceshipModel.SetTimeToRecoveryLaser(0f);
			SpaceshipModel.SetIsDead(true);
		}
	}
}