using UnityEngine;
using Weapon;

namespace Pools
{
    public class BulletPoolFacade : AbstractPool<Bullet>
    {
        public BulletPoolFacade(Bullet prefab, Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;
        }

        protected override void ActionOnGet(Bullet obj)
        {
            base.ActionOnGet(obj);
            obj.GetComponent<Collider2D>().enabled = true;
            obj.gameObject.SetActive(true);
        }

        protected override void ActionOnRelease(Bullet obj)
        {
            base.ActionOnRelease(obj);
            obj.gameObject.SetActive(false);
            obj.GetComponent<Collider2D>().enabled = false;
        }

        protected override void ActionOnDestroy(Bullet obj)
        {
            Object.Destroy(obj.gameObject);
            base.ActionOnDestroy(obj);
        }
    }
}