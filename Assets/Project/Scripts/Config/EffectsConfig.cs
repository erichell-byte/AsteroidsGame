using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Config
{
	[CreateAssetMenu(menuName = "ScriptableObjects/EffectsConfig")]
	public class EffectsConfig : ScriptableObject
	{
		public AssetReferenceGameObject ShotVfx;
		public AssetReferenceGameObject AsteroidExplosionVfx;
		public AssetReferenceGameObject UfoExplosionVfx;
		public int UfoExplosionDuration;
		public int AsteroidExplosionDuration;
		public int ShotDuration;
	}
}