using AssetsLoader;
using Enemies;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Pools
{
	public class EnemyPoolFacade : AbstractPool<Enemy>
	{
		public EnemyPoolFacade(
			IAssetLoader<Enemy> loader,
			AssetReferenceGameObject assetId,
			Transform parent = null)
			: base(loader, assetId, parent)
		{
		}

		protected override void OnGet(Enemy obj)
		{
			obj.GetComponent<Collider2D>().enabled = false;
			obj.gameObject.SetActive(false);
		}

		protected override void OnRelease(Enemy obj)
		{
			obj.GetComponent<Collider2D>().enabled = false;
			obj.gameObject.SetActive(false);
		}
	}
}