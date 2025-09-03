using AssetsLoader;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Pools
{
	public class EffectPoolFacade : AbstractPool<GameObject>
	{
		public EffectPoolFacade(
			IAssetLoader<GameObject> loader,
			AssetReferenceGameObject id,
			Transform parent = null)
			: base(loader, id, parent)
		{
		}

		protected override void OnGet(GameObject obj)
		{
			obj.SetActive(true);
		}

		protected override void OnRelease(GameObject obj)
		{
			obj.SetActive(false);
		}
	}
}