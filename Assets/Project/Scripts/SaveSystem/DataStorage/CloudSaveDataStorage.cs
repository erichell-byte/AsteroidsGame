using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Internal;
using Unity.Services.CloudSave.Models;

namespace SaveLoad
{
    public class CloudSaveDataStorage : IDataStorage
    {
        private static IPlayerDataService PlayerService =>
            CloudSaveService.Instance.Data.Player;

        public async UniTask<string> ReadAsync(string key)
        {
            var requestData = new HashSet<string> { key };

            Dictionary<string, Item> responseData =
                await PlayerService.LoadAsync(requestData);

            return responseData[key].Value.GetAsString();
        }

        public async UniTask WriteAsync(string key, string serializedData)
        {
            var requestData = new Dictionary<string, object>
            {
                { key, serializedData }
            };

            await PlayerService.SaveAsync(requestData);
        }

        public async UniTask DeleteAsync(string key)
        {
            await PlayerService.DeleteAsync(key);
        }

        public async UniTask<bool> ExistsAsync(string key)
        {
            List<ItemKey> responseData =
                await PlayerService.ListAllKeysAsync();

            return responseData.Select(d => d.Key).Any(k => k == key);
        }
    }
}