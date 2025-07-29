using UnityEngine;

namespace Utils
{
    public class ObjectTeleportByScreenBound : MonoBehaviour
    {
        private float _screenLeft;
        private float _screenRight;
        private float _screenTop;
        private float _screenBottom;

        public void Awake()
        {
            Camera mainCamera = Camera.main;

            Vector3 bottomLeft =
                mainCamera.ViewportToWorldPoint(new Vector3(0, 0,
                    transform.position.z - mainCamera.transform.position.z));
            Vector3 topRight =
                mainCamera.ViewportToWorldPoint(new Vector3(1, 1,
                    transform.position.z - mainCamera.transform.position.z));

            _screenLeft = bottomLeft.x;
            _screenRight = topRight.x;
            _screenBottom = bottomLeft.y;
            _screenTop = topRight.y;
        }

        private void Update()
        {
            Vector3 pos = transform.position;

            if (pos.x > _screenRight)
            {
                pos.x = _screenLeft;
            }
            else if (pos.x < _screenLeft)
            {
                pos.x = _screenRight;
            }

            if (pos.y > _screenTop)
            {
                pos.y = _screenBottom;
            }
            else if (pos.y < _screenBottom)
            {
                pos.y = _screenTop;
            }

            transform.position = pos;
        }
    }
}