using UnityEngine;

namespace Utils
{
	public class ObjectTeleportByScreenBound : MonoBehaviour
	{
		private float _screenBottom;
		private float _screenLeft;
		private float _screenRight;
		private float _screenTop;

		public void Awake()
		{
			var mainCamera = Camera.main;

			if (mainCamera != null)
			{
				var bottomLeft =
					mainCamera.ViewportToWorldPoint(new Vector3(0, 0,
						transform.position.z - mainCamera.transform.position.z));
				var topRight =
					mainCamera.ViewportToWorldPoint(new Vector3(1, 1,
						transform.position.z - mainCamera.transform.position.z));

				_screenLeft = bottomLeft.x;
				_screenRight = topRight.x;
				_screenBottom = bottomLeft.y;
				_screenTop = topRight.y;
			}
		}

		private void Update()
		{
			var pos = transform.position;

			if (pos.x > _screenRight)
				pos.x = _screenLeft;
			else if (pos.x < _screenLeft) pos.x = _screenRight;

			if (pos.y > _screenTop)
				pos.y = _screenBottom;
			else if (pos.y < _screenBottom) pos.y = _screenTop;

			transform.position = pos;
		}
	}
}