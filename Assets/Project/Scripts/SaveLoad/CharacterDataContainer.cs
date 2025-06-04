
namespace SaveLoad
{
    public class CharacterDataContainer : IDataContainer<CharacterStats>
    {
        public CharacterStats? Data { get; private set; }

        public void SetData(CharacterStats data)
        {
            Data = data;
        }

        public bool HasData()
        {
            return Data.HasValue;
        }
    }
}