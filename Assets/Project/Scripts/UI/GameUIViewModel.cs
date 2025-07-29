using Character;
using UniRx;
using UnityEngine;

namespace UI
{
    public class GameUIViewModel
    {
        private readonly SpaceshipModel _spaceshipModel;
        public IReadOnlyReactiveProperty<Vector2> Coordinate => _spaceshipModel.Position;
        public IReadOnlyReactiveProperty<float> RotationAngle => _spaceshipModel.Rotation;
        public IReadOnlyReactiveProperty<float> CurrentSpeed => _spaceshipModel.Speed;
        public IReadOnlyReactiveProperty<int> LaserCount => _spaceshipModel.LaserCount;
        public IReadOnlyReactiveProperty<float> TimeToResetLaser => _spaceshipModel.TimeToRecoveryLaser;
        public IReadOnlyReactiveProperty<bool> GameStartedButtonEnabled => _spaceshipModel.IsDead;
        public ReactiveCommand<Unit> StartGameButtonClickedCommand { get; } = new ();

        public GameUIViewModel(SpaceshipModel spaceshipModel)
        {
            this._spaceshipModel = spaceshipModel;
        }

        public void StartGameButtonClicked()
        {
            _spaceshipModel.SetIsDead(false);
            StartGameButtonClickedCommand.Execute(Unit.Default);
        }
    }
}
