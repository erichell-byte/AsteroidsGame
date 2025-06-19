using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AssetsLoader
{
    public class AddressablesBootstrap : MonoBehaviour
    {
        private async void Awake()
        {
            var initHandle = Addressables.InitializeAsync(false);
            await initHandle.Task.AsUniTask();
        }
    }
}