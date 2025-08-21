using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AssetsLoader
{
	public class AddressablesPreloader : IAssetsPreloader
	{
		private readonly List<AsyncOperationHandle> _loadedHandles = new ();
		public async UniTask PreloadAsync(IEnumerable<AssetReferenceGameObject> assets)
		{
			var tasks = new List<UniTask>();
			foreach (var asset in assets)
			{
				var handle = Addressables.LoadAssetAsync<UnityEngine.Object>(asset);
				_loadedHandles.Add(handle);
				tasks.Add(handle.Task.AsUniTask());
			}
			await UniTask.WhenAll(tasks);
		}

		public void ReleaseAll()
		{
			for (int i = 0; i < _loadedHandles.Count; i++)
			{
				var handle = _loadedHandles[i];
				if (handle.IsValid())
				{
					Addressables.Release(handle);
				}
			}
			_loadedHandles.Clear();
		}
	}
}

