using AssetsLoader;
using Pools;
using UnityEngine;
using UnityEngine.AddressableAssets;

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