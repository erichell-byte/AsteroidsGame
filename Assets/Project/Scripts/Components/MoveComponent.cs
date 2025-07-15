using Character;
using Config;
using Systems;
using UnityEngine;
using Zenject;

namespace Components
{
    public class MoveComponent : MonoBehaviour,
        IGameFinishListener,
        IGamePauseListener,
        ITickable
    {
        private GameConfigurationSO config;
        
        private Rigidbody2D rb2d;
        private float moveСoefficient;
        private float rotateCoefficient;
        private float maxVelocityMagnitude;
        private SpaceshipModel spaceshipModel;

        [Inject]
        private void Construct(
            GameCycle gameCycle,
            GameConfigurationSO config)
        {
            this.config = config;
            gameCycle.AddListener(this);
        }
        
        public void Initialize(
            SpaceshipModel spaceshipModel)
        {
            moveСoefficient = config.remoteConfig.moveCoefficient;
            rotateCoefficient = config.remoteConfig.rotateCoefficient;
            maxVelocityMagnitude = config.remoteConfig.maxVelocityMagnitude;
            this.spaceshipModel = spaceshipModel;
        }

        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
        }

        public void SetInitialPositionAndRotation(Vector3 position, float rotationZ)
        {
            transform.position = position;
            transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        }

        public void MoveForward()
        {
            rb2d.linearVelocity += (Vector2)transform.up * (Time.fixedDeltaTime * moveСoefficient);
            rb2d.linearVelocity = Vector2.ClampMagnitude(rb2d.linearVelocity, maxVelocityMagnitude);
            spaceshipModel?.SetSpeed(rb2d.linearVelocity.magnitude);
        }

        public void Rotate(int clockwiseDirection)
        {
            transform.Rotate(Vector3.back, clockwiseDirection * rotateCoefficient * Time.deltaTime);
            spaceshipModel?.SetRotation(transform.eulerAngles.z);
        }

        public void OnFinishGame()
        {
            rb2d.linearVelocity = Vector2.zero;
            transform.rotation = Quaternion.identity;
            transform.position = Vector3.zero;
        }
        
        public void OnPauseGame()
        {
            rb2d.linearVelocity = Vector2.zero;
        }

        public void Tick()
        {
            if (rb2d.linearVelocity.magnitude > 0)
            {
                spaceshipModel?.SetPosition(transform.position);
            }
        }
    }
}
