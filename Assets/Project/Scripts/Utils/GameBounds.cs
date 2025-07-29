using UnityEngine;

namespace Utils
{
    public class GameBounds : MonoBehaviour
    {
        private BoxCollider2D _boundaryCollider;

        private void Awake()
        {
            _boundaryCollider = GetComponent<BoxCollider2D>();
            UpdateBoundarySize();
        }

        private void UpdateBoundarySize()
        {
            if (_boundaryCollider == null) return;

            float height = Camera.main!.orthographicSize;
            float width = height * Camera.main.aspect;

            _boundaryCollider.size = new Vector2(width * 2, height * 2);
            _boundaryCollider.transform.position = Vector3.zero;
        }

        private void OnRectTransformDimensionsChange()
        {
            UpdateBoundarySize();
        }
    }
}