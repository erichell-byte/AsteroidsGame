using Character;
using Systems;
using UniRx;
using UnityEngine;

namespace UI
{
	public class GameUIViewModel
	{
		private readonly SpaceshipModel _spaceshipModel;
		private readonly ISceneLoader _sceneLoader;

		public GameUIViewModel(
			SpaceshipModel spaceshipModel,
			ISceneLoader sceneLoader)
		{
			_spaceshipModel = spaceshipModel;
			_sceneLoader = sceneLoader;
		}

		public IReadOnlyReactiveProperty<Vector2> Coordinate => _spaceshipModel.Position;
		public IReadOnlyReactiveProperty<float> RotationAngle => _spaceshipModel.Rotation;
		public IReadOnlyReactiveProperty<float> CurrentSpeed => _spaceshipModel.Speed;
		public IReadOnlyReactiveProperty<int> LaserCount => _spaceshipModel.LaserCount;
		public IReadOnlyReactiveProperty<float> TimeToResetLaser => _spaceshipModel.TimeToRecoveryLaser;
		public IReadOnlyReactiveProperty<bool> GameStartedButtonEnabled => _spaceshipModel.IsDead;

		public void StartGameButtonClicked()
		{
			_spaceshipModel.SetIsDead(false);
		}

		public async void FinishGameButtonClicked()
		{
			_spaceshipModel.SetIsDead(true);
			await _sceneLoader.LoadPreviousScene();
		}
	}
}