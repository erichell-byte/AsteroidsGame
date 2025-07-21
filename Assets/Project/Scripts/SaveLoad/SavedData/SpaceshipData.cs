using System;

namespace SaveLoad
{
    [Serializable]
    public class SpaceshipData : ISavedData
    {
        public float positionX;
        public float positionY;
        public float rotationZ;
    }
}