using System;
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

        public event Action<Vector2> OnCoordinateChanged;
        public event Action<float> OnRotationChanged;
        public event Action<float> OnSpeedChanged;

        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
        }

        public void Initialize(float moveСoefficient, float rotateCoefficient, float maxVelocityMagnitude)
        {
            this.moveСoefficient = moveСoefficient;
            this.rotateCoefficient = rotateCoefficient;
            this.maxVelocityMagnitude = maxVelocityMagnitude;
        }

        public void MoveForward()
        {
            rb2d.velocity += (Vector2)transform.up * (Time.fixedDeltaTime * moveСoefficient);
            rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxVelocityMagnitude);
            OnSpeedChanged?.Invoke(rb2d.velocity.magnitude);
        }

        public void Rotate(int clockwiseDirection)
        {
            transform.Rotate(Vector3.back, clockwiseDirection * rotateCoefficient * Time.deltaTime);
            OnRotationChanged?.Invoke(transform.eulerAngles.z);
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
                OnCoordinateChanged?.Invoke(transform.position);
            }
        }
    }
}