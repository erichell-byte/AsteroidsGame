using Enemies;

namespace Config
{
    [System.Serializable]
    public class EnemyConfig
    {
        public EnemyType Type;
        public float Speed;
        public float SpeedModifier = 1f;
    }
}