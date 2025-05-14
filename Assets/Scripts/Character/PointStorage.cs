using System;
using UnityEngine;

namespace Character
{
    public class PointStorage : MonoBehaviour
    {
        public event Action<int> OnPointChanged;

        public int Points { get; private set; }

        public PointStorage(int points)
        {
            Points = points;
        }

        public void AddPoints(int points)
        {
            Points += points;
            OnPointChanged?.Invoke(Points);
        }

        public void ClearPoints()
        {
            Points = 0;
            OnPointChanged?.Invoke(Points);
        }
    }
}