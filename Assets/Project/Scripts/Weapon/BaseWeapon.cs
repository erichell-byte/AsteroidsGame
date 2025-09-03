using UnityEngine;

namespace Weapon
{
	public abstract class BaseWeapon
	{
		protected Transform ShotPoint;

		public abstract void Attack();
	}
}