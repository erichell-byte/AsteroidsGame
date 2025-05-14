using UnityEngine;
using UnityEngine.Pool;

namespace Pools
{
    public class AbstractPool<T> where T : Object
    {
        private ObjectPool<T> pool;

        protected T prefab;
        protected Transform parent;

        public ObjectPool<T> Pool => pool;

        public AbstractPool()
        {
            pool = new ObjectPool<T>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy, true, 10, 30);
        }

        protected T CreateFunc()
        {
            return Object.Instantiate(prefab, parent);
        }

        protected virtual void ActionOnGet(T obj)
        {
        }

        protected virtual void ActionOnRelease(T obj)
        {
        }

        protected virtual void ActionOnDestroy(T obj)
        {
        }
    }
}