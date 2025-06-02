using System;
using Enemies;
using UnityEngine;
using Utils;

namespace Weapon
{
    public class Bullet : MonoBehaviour
    {
        public Action<Bullet> OnHit;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.TryGetComponent(out Enemy enemy))
            {
                enemy.Die();
                OnHit?.Invoke(this);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.gameObject.GetComponent<GameBounds>())
                OnHit?.Invoke(this);
        }
    }
}