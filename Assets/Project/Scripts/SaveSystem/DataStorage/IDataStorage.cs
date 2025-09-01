using Cysharp.Threading.Tasks;

namespace SaveLoad
{
	public interface IDataStorage
	{
		UniTask<string> ReadAsync(string key);
		UniTask WriteAsync(string key, string serializedData);
		UniTask DeleteAsync(string key);
		UniTask<bool> ExistsAsync(string key);
	}
}