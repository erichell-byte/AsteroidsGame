using AssetsLoader;
using Enemies;
using UnityEngine;

namespace Pools
{
    public class EnemyPoolFacade : AbstractPool<Enemy>
    {
        public EnemyPoolFacade(IAssetLoader<Enemy> loader, string assetId, Transform parent = null)
            : base(loader, assetId, parent)
        {
        }

        protected override void OnGet(Enemy obj)
        {
            obj.gameObject.SetActive(true);
            obj.GetComponent<Collider2D>().enabled = true;
        }

        protected override void OnRelease(Enemy obj)
        {
            obj.GetComponent<Collider2D>().enabled = false;
            obj.gameObject.SetActive(false);
        }
    }
}