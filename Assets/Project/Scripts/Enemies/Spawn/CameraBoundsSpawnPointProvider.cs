using UnityEngine;

namespace Enemies
{
	public class CameraBoundsSpawnPointProvider : IEnemySpawnPointProvider
	{
		public Vector3 GetSpawnPoint()
		{
			var cam = Camera.main;
			float height = cam!.orthographicSize;
			float width = height * cam.aspect;
			switch (Random.Range(0, 4))
			{
				case 0: return new Vector3(-width - 2, 0, 0);
				case 1: return new Vector3(width + 2, 0, 0);
				case 2: return new Vector3(0, height + 2, 0);
				default: return new Vector3(0, -height - 2, 0);
			}
		}
	}
}