using System;
using Config;
using UnityEngine;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        protected EnemyConfig config;
        protected Rigidbody2D rb;

        public Action<Enemy> OnDeath;

        public virtual void Initialize(EnemyConfig config)
        {
            this.config = config;
            rb = GetComponent<Rigidbody2D>();
        }

        public virtual void Die()
        {
            OnDeath?.Invoke(this);
        }

        public int GetPoints()
        {
            return config.PointsReward;
        }
    }
}