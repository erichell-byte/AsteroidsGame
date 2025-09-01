using Analytics;
using Character;
using Components;
using Enemies;
using Sounds;
using Systems;
using UI;
using UnityEngine;
using Utils;
using Zenject;

namespace Installers
{
	public class GameSceneInstaller : MonoInstaller
	{
		[SerializeField] private AttackComponent _attackComponent;
		[SerializeField] private MoveComponent _moveComponent;
		[SerializeField] private CollisionComponent _collisionComponent;
		[SerializeField] private Transform _poolParent;
		[SerializeField] private EnemiesManager _enemiesManager;
		[SerializeField] private AudioSource _musicAudioSource;
		[SerializeField] private AudioSource _sfxAudioSource;

		[Header("UI")]
		[SerializeField] private GameUIView _gameUIView;
		[SerializeField] private AdView _adView;

		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<GameEventsController>().AsSingle();
			Container.Bind<CollisionComponent>().FromInstance(_collisionComponent);
			Container.BindFactory<Timer, Timer.Factory>().FromNew();
			Container.BindInterfacesAndSelfTo<TimersController>().AsSingle();
			Container.BindInterfacesAndSelfTo<AttackComponent>().FromInstance(_attackComponent).AsSingle();
			Container.BindInterfacesAndSelfTo<MoveComponent>().FromInstance(_moveComponent).AsSingle();
			Container.BindInterfacesAndSelfTo<SpaceshipController>().AsSingle();
			Container.Bind<Transform>().FromInstance(_poolParent).AsSingle();
			Container.Bind<GameUIView>().FromInstance(_gameUIView).AsSingle();
			Container.Bind<AdView>().FromInstance(_adView).AsSingle();
			Container.Bind<UIController>().AsSingle().NonLazy();
			Container.Bind<IAnalyticsHandler>().To<FirebaseAnalyticsHandler>().AsSingle();
			Container.Bind<EnemiesManager>().FromInstance(_enemiesManager).AsSingle();
			Container.BindInterfacesAndSelfTo<AnalyticsMediator>().AsSingle();
			Container.Bind<EffectsMediator>().AsSingle().NonLazy();

			Container.Bind<AudioSource>().WithId(AudioSourceId.Music).FromInstance(_musicAudioSource).NonLazy();
			Container.Bind<AudioSource>().WithId(AudioSourceId.Sfx).FromInstance(_sfxAudioSource).NonLazy();
			Container.Bind<AudioMediator>().AsSingle().NonLazy();
		}
	}
	
	public enum AudioSourceId
	{
		Music,
		Sfx
	}
}