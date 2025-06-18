using System.Threading.Tasks;

namespace AssetsLoader
{
    public interface IAssetLoader<T>
    {
        Task<T> LoadAsset(string assetId);
        void Unload(T obj);
    }
}