using Enemies;
using UnityEngine;

namespace Pools
{
    public class EnemyPoolFacade : AbstractPool<Enemy>
    {
        public EnemyPoolFacade(Enemy prefab, Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;
        }

        protected override void ActionOnGet(Enemy obj)
        {
            obj.gameObject.SetActive(true);
            obj.GetComponent<Collider2D>().enabled = true;
            base.ActionOnGet(obj);
        }

        protected override void ActionOnRelease(Enemy obj)
        {
            obj.GetComponent<Collider2D>().enabled = false;
            obj.gameObject.SetActive(false);
            base.ActionOnRelease(obj);
        }

        protected override void ActionOnDestroy(Enemy obj)
        {
            Object.Destroy(obj.gameObject);
            base.ActionOnDestroy(obj);
        }
    }
}