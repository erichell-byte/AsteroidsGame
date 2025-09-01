using UnityEngine;

namespace Weapon
{
	public abstract class BaseWeapon
	{
		protected Transform shotPoint;

		public abstract void Attack();
	}
}