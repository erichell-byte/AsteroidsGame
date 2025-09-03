using UnityEngine;

namespace Enemies
{
	public interface IEnemySpawnPointProvider
	{
		Vector3 GetSpawnPoint();
	}
}