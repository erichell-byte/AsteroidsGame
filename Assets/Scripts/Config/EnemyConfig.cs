using Enemies;

namespace Config
{
    [System.Serializable]
    public class EnemyConfig
    {
        public EnemyType type;
        public float speed;
        public float speedModifier = 1f;
    }
}