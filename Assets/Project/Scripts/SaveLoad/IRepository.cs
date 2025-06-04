namespace SaveLoad
{
    public interface IRepository<T> where T : struct
    {
        bool Load(out T data);
        void Save(T data);
    }
}