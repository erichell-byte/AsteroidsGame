using System;

namespace SaveLoad
{
    [Serializable]
    public class SpaceshipData : ISavedData
    {
        public float PositionX;
        public float PositionY;
        public float RotationZ;
    }
}