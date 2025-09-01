using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Zenject;

namespace AssetsLoader
{
	public class AddressablesBootstrap : IInitializable
	{
		public void Initialize()
		{
			var initHandle = Addressables.InitializeAsync(false);
			initHandle.Task.AsUniTask().Forget();
		}
	}
}