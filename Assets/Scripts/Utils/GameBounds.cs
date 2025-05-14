using UnityEngine;

namespace Utils
{
    public class GameBounds : MonoBehaviour
    {
        private BoxCollider2D boundaryCollider;

        private void Awake()
        {
            boundaryCollider = GetComponent<BoxCollider2D>();
            UpdateBoundarySize();
        }

        private void UpdateBoundarySize()
        {
            if (boundaryCollider == null) return;

            float height = Camera.main.orthographicSize;
            float width = height * Camera.main.aspect;

            boundaryCollider.size = new Vector2(width * 2, height * 2);
            boundaryCollider.transform.position = Vector3.zero;
        }

        private void OnRectTransformDimensionsChange()
        {
            UpdateBoundarySize();
        }
    }
}