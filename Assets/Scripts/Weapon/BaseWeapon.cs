using UnityEngine;

namespace Weapon
{
    public abstract class BaseWeapon
    {
        protected readonly Transform ShotPoint;

        protected BaseWeapon(Transform shotPoint)
        {
            ShotPoint = shotPoint;
        }

        public abstract void Attack();
    }
}