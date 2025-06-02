using UnityEngine;

namespace Utils
{
    public class ObjectTeleportByScreenBound : MonoBehaviour
    {
        private float screenLeft;
        private float screenRight;
        private float screenTop;
        private float screenBottom;

        public void Awake()
        {
            Camera mainCamera = Camera.main;

            Vector3 bottomLeft =
                mainCamera.ViewportToWorldPoint(new Vector3(0, 0,
                    transform.position.z - mainCamera.transform.position.z));
            Vector3 topRight =
                mainCamera.ViewportToWorldPoint(new Vector3(1, 1,
                    transform.position.z - mainCamera.transform.position.z));

            screenLeft = bottomLeft.x;
            screenRight = topRight.x;
            screenBottom = bottomLeft.y;
            screenTop = topRight.y;
        }

        private void Update()
        {
            Vector3 pos = transform.position;

            if (pos.x > screenRight)
            {
                pos.x = screenLeft;
            }
            else if (pos.x < screenLeft)
            {
                pos.x = screenRight;
            }

            if (pos.y > screenTop)
            {
                pos.y = screenBottom;
            }
            else if (pos.y < screenBottom)
            {
                pos.y = screenTop;
            }

            transform.position = pos;
        }
    }
}