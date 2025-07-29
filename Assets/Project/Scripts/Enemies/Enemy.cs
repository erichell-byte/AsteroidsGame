using System;
using Config;
using UnityEngine;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        protected EnemyConfig Config;
        protected Rigidbody2D Rb;
        protected EnemyType EnemyType;
        protected bool IsActive;

        public Action<Enemy> OnDeath;

        public virtual void Initialize(EnemyConfig config)
        {
            this.Config = config;
            EnemyType = config.Type;
            Rb = GetComponent<Rigidbody2D>();
        }

        public virtual void Die()
        {
            OnDeath?.Invoke(this);
        }

        public EnemyType GetEnemyType()
        {
            return EnemyType;
        }
        
        public virtual void SetActive(bool isActive)
        {
            this.IsActive = isActive;
        }
    }
}