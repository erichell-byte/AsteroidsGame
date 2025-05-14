using System;
using UnityEngine;

namespace Systems
{
    public class KeyboardInputReceiver : MonoBehaviour,
        IGameUpdateListener
    {
        public Action<int> InputRotationValue;
        public Action InputMoveValue;
        public Action InputMainShotValue;
        public Action InputAdditionalShotValue;

        public void OnUpdate(float deltaTime)
        {
            if (Input.GetKey(KeyCode.A))
            {
                InputRotationValue.Invoke(-1);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                InputRotationValue.Invoke(1);
            }

            if (Input.GetKey(KeyCode.W))
            {
                InputMoveValue.Invoke();
            }

            if (Input.GetKey(KeyCode.Space))
            {
                InputMainShotValue.Invoke();
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                InputAdditionalShotValue.Invoke();
            }
        }
    }
}