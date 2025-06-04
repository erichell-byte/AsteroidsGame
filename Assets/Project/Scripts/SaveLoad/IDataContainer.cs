
namespace SaveLoad
{
    public interface IDataContainer<T> where T : struct
    {
        T? Data { get; }
        void SetData(T data);
        bool HasData();
    }
}