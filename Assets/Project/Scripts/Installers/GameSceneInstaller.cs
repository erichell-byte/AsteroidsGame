using Components;
using Analytics;
using AssetsLoader;
using Character;
using Enemies;
using Systems;
using UI;
using UnityEngine;
using Utils;
using Weapon;
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
            Container.Bind<IGameEvents>().To<GameEvents>().AsSingle();
        
        }
    }
}