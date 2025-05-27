using System;
using Config;
using UnityEngine;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        protected EnemyConfig config;
        protected Rigidbody2D rb;
        protected EnemyType enemyType;

        public Action<Enemy> OnDeath;

        public virtual void Initialize(EnemyConfig config)
        {
            this.config = config;
            enemyType = config.type;
            rb = GetComponent<Rigidbody2D>();
        }

        public virtual void Die()
        {
            OnDeath?.Invoke(this);
        }

        public EnemyType GetEnemyType()
        {
            return enemyType;
        }
    }
}