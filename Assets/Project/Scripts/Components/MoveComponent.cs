using Character;
using Config;
using Systems;
using UnityEngine;
using Zenject;

namespace Components
{
    public class MoveComponent : MonoBehaviour, IGameFinishListener, ITickable
    {
        private GameConfiguration config;
        
        private Rigidbody2D rb2d;
        private float moveСoefficient;
        private float rotateCoefficient;
        private float maxVelocityMagnitude;
        private CharacterModel characterModel;

        [Inject]
        private void Construct(
            GameCycle gameCycle,
            GameConfiguration config)
        {
            this.config = config;
            gameCycle.AddListener(this);
        }
        
        public void Initialize(
            CharacterModel characterModel)
        {
            moveСoefficient = config.moveCoefficient;
            rotateCoefficient = config.rotateCoefficient;
            maxVelocityMagnitude = config.maxVelocityMagnitude;
            this.characterModel = characterModel;
        }

        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
        }

        public void MoveForward()
        {
            rb2d.linearVelocity += (Vector2)transform.up * (Time.fixedDeltaTime * moveСoefficient);
            rb2d.linearVelocity = Vector2.ClampMagnitude(rb2d.linearVelocity, maxVelocityMagnitude);
            characterModel?.SetSpeed(rb2d.linearVelocity.magnitude);
        }

        public void Rotate(int clockwiseDirection)
        {
            transform.Rotate(Vector3.back, clockwiseDirection * rotateCoefficient * Time.deltaTime);
            characterModel?.SetRotation(transform.eulerAngles.z);
        }

        public void OnFinishGame()
        {
            rb2d.linearVelocity = Vector2.zero;
            transform.rotation = Quaternion.identity;
            transform.position = Vector3.zero;
        }

        public void Tick()
        {
            if (rb2d.linearVelocity.magnitude > 0)
            {
                characterModel?.SetPosition(transform.position);
            }
        }
    }
}
