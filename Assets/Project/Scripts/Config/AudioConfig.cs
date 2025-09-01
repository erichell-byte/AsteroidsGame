using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AudioConfig")]
public class AudioConfig : ScriptableObject
{
	public AudioClip ShotSfx;
	public AudioClip AsteroidExplosionSfx;
	public AudioClip UfoExplosionSfx;
	public AudioClip BackgroundMusic;
	public float SfxVolume = 1f;
	public float BackgroundVolume = 0.5f;
}