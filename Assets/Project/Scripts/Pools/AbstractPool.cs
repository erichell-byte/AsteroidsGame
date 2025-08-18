using System.Collections.Generic;
using AssetsLoader;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Pools
{
    public class AbstractPool<T> where T : Object
    {
        private readonly Stack<T> _inactive = new ();
        private readonly AssetReferenceGameObject _assetId;
        private readonly int _maxSize;
        private readonly IAssetLoader<T> _loader;
        private readonly Transform _parent;
        
        protected AbstractPool(IAssetLoader<T> loader, AssetReferenceGameObject assetId, Transform parent, int maxSize = 100)
        {
            _assetId = assetId;
            _parent = parent;
            _maxSize = maxSize;
            _loader = loader;
        }

        public async UniTask<T> GetAsync()
        {
            if (_inactive.Count > 0)
            {
                var obj = _inactive.Pop();
                OnGet(obj);
                return obj;
            }

            var objNew = await _loader.InstantiateAsset(_assetId);
            if (objNew is Component component && _parent != null)
                component.transform.SetParent(_parent);
            else if (objNew is GameObject go && _parent != null)
                go.transform.SetParent(_parent);

            OnGet(objNew);
            return objNew;
        }

        protected virtual void OnGet(T obj) { }
        protected virtual void OnRelease(T obj) { }
        protected virtual void OnDestroy(T obj) => _loader.Unload(obj);

        public void Release(T obj)
        {
            if (obj == null) return;
            if (_inactive.Count >= _maxSize)
                OnDestroy(obj);
            else
            {
                OnRelease(obj);
                _inactive.Push(obj);
            }
        }

        public void Clear()
        {
            while (_inactive.Count > 0)
                OnDestroy(_inactive.Pop());
        }
    }
}