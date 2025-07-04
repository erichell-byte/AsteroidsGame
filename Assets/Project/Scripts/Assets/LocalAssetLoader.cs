using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AssetsLoader
{
    public class LocalAssetLoader<T> : IAssetLoader<T>
    {
        public async UniTask<T> InstantiateAsset(AssetReferenceGameObject assetId)
        {
            var handle = Addressables.InstantiateAsync(assetId);
            GameObject go = await handle.Task.AsUniTask();
            go.SetActive(false);
            if (go.TryGetComponent<T>(out var component))
                return component;
            if (go is T obj)
                return obj;
            Addressables.ReleaseInstance(go);
            throw new NullReferenceException($"Asset '{assetId}' missing component {typeof(T)}");
        }
        
        public void Unload(T obj)
        {
            if (obj is Component comp)
            {
                comp.gameObject.SetActive(false);
                Addressables.ReleaseInstance(comp.gameObject);
            }
            else if (obj is GameObject go)
            {
                go.SetActive(false);
                Addressables.ReleaseInstance(go);
            }
        }
    }
}