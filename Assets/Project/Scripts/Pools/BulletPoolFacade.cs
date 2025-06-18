using AssetsLoader;
using UnityEngine;
using Weapon;

namespace Pools
{
    public class BulletPoolFacade : AbstractPool<Bullet>
    {
        public BulletPoolFacade(IAssetLoader<Bullet> loader, string assetId, Transform parent = null)
            : base(loader, assetId, parent) { }

        protected override void OnGet(Bullet obj)
        {
            obj.gameObject.SetActive(true);
            obj.GetComponent<Collider2D>().enabled = true;
        }

        protected override void OnRelease(Bullet obj)
        {
            obj.gameObject.SetActive(false);
            obj.GetComponent<Collider2D>().enabled = false;
        }
    }
}