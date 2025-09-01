using Cysharp.Threading.Tasks;

namespace SaveLoad
{
	public interface ISerializer
	{
		UniTask<string> SerializeAsync<TData>(TData data);
		UniTask<TData> DeserializeAsync<TData>(string serializedData);
	}
}