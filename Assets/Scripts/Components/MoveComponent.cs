using System;
using Character;
using Config;
using Systems;
using UnityEngine;

namespace Components
{
    public class MoveComponent : MonoBehaviour, IGameFinishListener, IGameUpdateListener
    {
        private Rigidbody2D rb2d;
        private float moveСoefficient;
        private float rotateCoefficient;
        private float maxVelocityMagnitude;

        private CharacterModel characterModel;

        public void Initialize(GameConfiguration config,
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
            rb2d.velocity += (Vector2)transform.up * (Time.fixedDeltaTime * moveСoefficient);
            rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxVelocityMagnitude);
            characterModel?.SetSpeed(rb2d.velocity.magnitude);
        }

        public void Rotate(int clockwiseDirection)
        {
            transform.Rotate(Vector3.back, clockwiseDirection * rotateCoefficient * Time.deltaTime);
            characterModel?.SetRotation(transform.eulerAngles.z);
        }

        public void OnFinishGame()
        {
            rb2d.velocity = Vector2.zero;
            transform.rotation = Quaternion.identity;
            transform.position = Vector3.zero;
        }

        public void OnUpdate(float deltaTime)
        {
            if (rb2d.velocity.magnitude > 0)
            {
                characterModel?.SetPosition(transform.position);
            }
        }
    }
}
