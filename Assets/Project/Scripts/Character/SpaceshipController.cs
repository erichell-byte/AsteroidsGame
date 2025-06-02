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
        IGameFinishListener,
        ITickable
    {
        private GameConfiguration config;
        private MoveComponent moveComponent;
        private AttackComponent attackComponent;
        private Transform poolParent;
        
        private KeyboardInputReceiver inputReceiver;
        private CharacterModel characterModel;
        
        public CharacterModel CharacterModel => characterModel;

        [Inject]
        private void Construct(
            GameConfiguration config,
            MoveComponent moveComponent,
            AttackComponent attackComponent,
            Transform poolParent,
            GameCycle gameCycle)
        {
            this.config = config;
            this.moveComponent = moveComponent;
            this.attackComponent = attackComponent;
            this.poolParent = poolParent;
            inputReceiver = new KeyboardInputReceiver();
            
            gameCycle.AddListener(this);
            
            characterModel = new CharacterModel(
                moveComponent.transform.position,
                0f,
                0f,
                config.countOfLaserShots,
                0f);
        }

        public void OnStartGame()
        {
            moveComponent.Initialize(characterModel);
            attackComponent.Initialize(characterModel);

            inputReceiver.InputMoveValue += moveComponent.MoveForward;
            inputReceiver.InputRotationValue += moveComponent.Rotate;

            inputReceiver.InputMainShotValue += attackComponent.AttackByMainShot;
            inputReceiver.InputAdditionalShotValue += attackComponent.AttackByLaserShot;
        }
        
        public void ResetCharacterModel()
        {
            characterModel.SetPosition(Vector3.zero);
            characterModel.SetRotation(0f);
            characterModel.SetSpeed(0f);
            characterModel.SetLaserCount(config.countOfLaserShots);
            characterModel.SetTimeToRecoveryLaser(0f);
            characterModel.SetIsDead(true);
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
    }
}