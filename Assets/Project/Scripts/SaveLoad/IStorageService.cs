namespace SaveLoad
{
    public interface IStorageService
    {
        bool HasKey(string key);
        string GetString(string key);
        void SetString(string key, string value);
    }
}