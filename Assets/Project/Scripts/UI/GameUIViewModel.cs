using UniRx;
using UnityEngine;

namespace Character
{
    public class GameUIViewModel
    {
        private SpaceshipModel spaceshipModel;
        public IReadOnlyReactiveProperty<Vector2> Coordinate => spaceshipModel.Position;
        public IReadOnlyReactiveProperty<float> RotationAngle => spaceshipModel.Rotation;
        public IReadOnlyReactiveProperty<float> CurrentSpeed => spaceshipModel.Speed;
        public IReadOnlyReactiveProperty<int> LaserCount => spaceshipModel.LaserCount;
        public IReadOnlyReactiveProperty<float> TimeToResetLaser => spaceshipModel.TimeToRecoveryLaser;
        public IReadOnlyReactiveProperty<bool> GameStartedButtonEnabled => spaceshipModel.IsDead;
        public ReactiveCommand<Unit> StartGameButtonClickedCommand { get; } = new ();

        public GameUIViewModel(SpaceshipModel spaceshipModel)
        {
            this.spaceshipModel = spaceshipModel;
        }

        public void StartGameButtonClicked()
        {
            spaceshipModel.SetIsDead(false);
            StartGameButtonClickedCommand.Execute(Unit.Default);
        }
    }
}
