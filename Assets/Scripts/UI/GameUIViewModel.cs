using UniRx;
using UnityEngine;

namespace Character
{
    public class GameUIViewModel
    {
        private CharacterModel characterModel;
        public IReadOnlyReactiveProperty<Vector2> Coordinate => characterModel.Position;
        public IReadOnlyReactiveProperty<float> RotationAngle => characterModel.Rotation;
        public IReadOnlyReactiveProperty<float> CurrentSpeed => characterModel.Speed;
        public IReadOnlyReactiveProperty<int> LaserCount => characterModel.LaserCount;
        public IReadOnlyReactiveProperty<float> TimeToResetLaser => characterModel.TimeToRecoveryLaser;
        public IReadOnlyReactiveProperty<bool> GameStartedButtonEnabled => characterModel.IsDead;
        public ReactiveCommand<Unit> StartGameButtonClickedCommand { get; } = new ();

        public GameUIViewModel(CharacterModel characterModel)
        {
            this.characterModel = characterModel;
        }

        public void StartGameButtonClicked()
        {
            characterModel.SetIsDead(false);
            StartGameButtonClickedCommand.Execute(Unit.Default);
        }
    }
}
