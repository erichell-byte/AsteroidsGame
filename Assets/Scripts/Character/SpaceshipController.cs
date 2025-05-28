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
        private CharacterModel characterModel;
        
        public CharacterModel CharacterModel => characterModel;

        [Inject]
        private void Construct(GameConfiguration config)
        {
            this.config = config;
            
            moveComponent = GetComponent<MoveComponent>();
            attackComponent = GetComponent<AttackComponent>();
            
            characterModel = new CharacterModel(
                moveComponent.transform.position,
                0f,
                0f,
                config.countOfLaserShots,
                0f);
        }

        public void OnStartGame()
        {
            moveComponent.Initialize(config, characterModel);
            attackComponent.Initialize(config, poolParent, characterModel);

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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Enemy>())
            {
                characterModel.SetIsDead(true);
            }
        }
    }
}