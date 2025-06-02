using System;
using UnityEngine;

namespace Input
{
    public class KeyboardInputReceiver
    {
        public Action<int> InputRotationValue;
        public Action InputMoveValue;
        public Action InputMainShotValue;
        public Action InputAdditionalShotValue;

        public void Tick()
        {
            if (UnityEngine.Input.GetKey(KeyCode.A))
            {
                InputRotationValue?.Invoke(-1);
            }
            else if (UnityEngine.Input.GetKey(KeyCode.D))
            {
                InputRotationValue?.Invoke(1);
            }

            if (UnityEngine.Input.GetKey(KeyCode.W))
            {
                InputMoveValue?.Invoke();
            }

            if (UnityEngine.Input.GetKey(KeyCode.Space))
            {
                InputMainShotValue?.Invoke();
            }

            if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
            {
                InputAdditionalShotValue?.Invoke();
            }
        }
    }
}