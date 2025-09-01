using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace AssetsLoader
{
	public interface IAssetsPreloader
	{
		UniTask PreloadAsync(IEnumerable<AssetReferenceGameObject> assets);
		void ReleaseAll();
	}
}