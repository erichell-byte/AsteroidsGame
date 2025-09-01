using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace AssetsLoader
{
	public interface IAssetLoader<T>
	{
		UniTask<T> InstantiateAsset(AssetReferenceGameObject assetId);
		void Unload(T obj);
	}
}