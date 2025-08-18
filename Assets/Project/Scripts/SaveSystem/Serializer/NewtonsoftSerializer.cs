using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

namespace SaveLoad
{
    public class NewtonsoftSerializer : ISerializer
    {
        public UniTask<string> SerializeAsync<TData>(TData data)
        {
            string json = JsonConvert.SerializeObject(data);
            return UniTask.FromResult(json);
        }

        public UniTask<TData> DeserializeAsync<TData>(string serializedData)
        {
            var data =JsonConvert.DeserializeObject<TData>(serializedData);
            return UniTask.FromResult(data);
        }
    }
}