using System;
using Config;
using Enemies;
using Installers;
using Systems;
using UniRx;
using UnityEngine;
using Zenject;

namespace Sounds
{
	public class AudioMediator : IGameStartListener, IGameFinishListener, IDisposable
	{
		private readonly AudioConfig _audioConfig;
		private readonly AudioSource _musicAudioSource;
		private readonly AudioSource _sfxAudioSource;
		private readonly CompositeDisposable _disposables = new();

		[Inject]
		public AudioMediator(
			[Inject(Id = AudioSourceId.Music)] AudioSource musicAudioSource,
			[Inject(Id = AudioSourceId.Sfx)] AudioSource sfxAudioSource,
			IGameEvents events,
			GameConfiguration gameConfiguration,
			GameCycle gameCycle)
		{
			_musicAudioSource = musicAudioSource;
			_sfxAudioSource = sfxAudioSource;
			_audioConfig = gameConfiguration.Audio;

			events.OnSpaceshipShot
				.Subscribe(_ => PlayShotMusic())
				.AddTo(_disposables);
			events.OnEnemyKilled
				.Subscribe(PlayEnemyKilledSound)
				.AddTo(_disposables);

			gameCycle.AddListener(this);
		}

		public void OnFinishGame()
		{
			_sfxAudioSource.Stop();
			_musicAudioSource.Stop();
		}

		public void OnStartGame()
		{
			_sfxAudioSource.clip = _audioConfig.BackgroundMusic;
			_sfxAudioSource.volume = _audioConfig.BackgroundVolume;
			_sfxAudioSource.Play();
			_musicAudioSource.Play();
		}

		private void PlayShotMusic()
		{
			_musicAudioSource.PlayOneShot(_audioConfig.ShotSfx, _audioConfig.SfxVolume);
		}

		private void PlayEnemyKilledSound((Vector3 pos, EnemyType type) tuple)
		{
			_musicAudioSource.PlayOneShot(
				tuple.Item2 == EnemyType.UFO
					? _audioConfig.UfoExplosionSfx
					: _audioConfig.AsteroidExplosionSfx, _audioConfig.SfxVolume);
		}

		public void Dispose()
		{
			_disposables?.Clear();
		}
	}
}