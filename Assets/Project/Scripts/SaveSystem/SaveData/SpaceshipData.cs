using System;

namespace SaveLoad
{
	[Serializable]
	public class SpaceshipData : ISaveData
	{
		public float PositionX;
		public float PositionY;
		public float RotationZ;
		public long Timestamp { get; set; }
	}
}