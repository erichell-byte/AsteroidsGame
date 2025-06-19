using System.Collections.Generic;
using AssetsLoader;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Pools
{
    public class AbstractPool<T> where T : Object
    {
        private readonly Stack<T> inactive = new ();
        private readonly AssetReferenceGameObject assetId;
        private readonly int maxSize;
        private readonly IAssetLoader<T> loader;
        private readonly Transform parent;
        

        protected AbstractPool(IAssetLoader<T> loader, AssetReferenceGameObject assetId, Transform parent, int maxSize = 100)
        {
            this.assetId = assetId;
            this.parent = parent;
            this.maxSize = maxSize;
            this.loader = loader;
        }

        public async UniTask<T> GetAsync()
        {
            if (inactive.Count > 0)
            {
                var obj = inactive.Pop();
                OnGet(obj);
                return obj;
            }

            var objNew = await loader.InstantiateAsset(assetId);
            if (objNew is Component component && parent != null)
                component.transform.SetParent(parent);
            else if (objNew is GameObject go && parent != null)
                go.transform.SetParent(parent);

            OnGet(objNew);
            return objNew;
        }

        protected virtual void OnGet(T obj) { }
        protected virtual void OnRelease(T obj) { }
        protected virtual void OnDestroy(T obj) => loader.Unload(obj);

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