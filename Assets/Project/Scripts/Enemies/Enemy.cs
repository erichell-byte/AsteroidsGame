using System;
using Config;
using UnityEngine;

namespace Enemies
{
	public abstract class Enemy : MonoBehaviour
	{
		protected EnemyConfig Config;
		protected EnemyType EnemyType;
		protected bool IsActive;

		public Action<Enemy> OnDeath;
		protected Rigidbody2D Rb;

		public virtual void Initialize(EnemyConfig config)
		{
			Config = config;
			EnemyType = config.Type;
			Rb = GetComponent<Rigidbody2D>();
			
			gameObject.SetActive(true);
			gameObject.GetComponent<Collider2D>().enabled = true;
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
			IsActive = isActive;
		}
	}
}