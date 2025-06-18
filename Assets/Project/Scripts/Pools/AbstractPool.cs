using System.Collections.Generic;
using System.Threading.Tasks;
using AssetsLoader;
using UnityEngine;

namespace Pools
{
    public class AbstractPool<T> where T : Object
    {
        private readonly Stack<T> inactive;
        private readonly string assetId;
        private readonly int maxSize;
        private readonly IAssetLoader<T> loader;

        protected Transform parent;
        

        protected AbstractPool(IAssetLoader<T> loader, string assetId, Transform parent, int maxSize = 100)
        {
            this.assetId = assetId;
            this.parent = parent;
            this.maxSize = maxSize;
            this.loader = loader;
            inactive = new Stack<T>(10);
        }

        private async Task<T> CreateFuncAsync()
        {
            T asset = await loader.LoadAsset(assetId);
            if (asset is Component component && parent != null)
                component.transform.SetParent(parent);
            else if (asset is GameObject go && parent != null)
                go.transform.SetParent(parent);
            return asset;
        }

        protected virtual void OnGet(T obj)
        {
        }

        protected virtual void OnRelease(T obj)
        {
        }

        protected virtual void OnDestroy(T obj) => loader.Unload(obj);

        public async Task<T> GetAsync()
        {
            T obj;
            if (inactive.Count > 0)
                obj = inactive.Pop();
            else
                obj = await CreateFuncAsync();
            OnGet(obj);
            return obj;
        }

        public void Release(T obj)
        {
            if (obj == null) return;
            if (inactive.Count >= maxSize)
                OnDestroy(obj);
            else
            {
                OnRelease(obj);
                inactive.Push(obj);
            }
        }

        public void Clear()
        {
            while (inactive.Count > 0)
                OnDestroy(inactive.Pop());
        }
    }
}