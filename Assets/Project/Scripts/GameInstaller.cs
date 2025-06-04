using Character;
using Components;
using Config;
using GameSystem;
using Analytics;
using SaveLoad;
using Systems;
using UI;
using UnityEngine;
using Utils;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameConfiguration gameConfiguration;
    [SerializeField] private AttackComponent attackComponent;
    [SerializeField] private MoveComponent moveComponent;
    [SerializeField] private CollisionComponent collisionComponent;
    [SerializeField] private Transform poolParent;
    [SerializeField] private GameUIView gameUIView;
    
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameCycle>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GameEventsController>().AsSingle().NonLazy();
        Container.Bind<CollisionComponent>().FromInstance(collisionComponent);
        Container.BindFactory<Timer, Timer.Factory>().FromNew();
        Container.Bind<GameConfiguration>().FromInstance(gameConfiguration);
        Container.BindInterfacesAndSelfTo<TimersController>().AsSingle();
        Container.BindInterfacesAndSelfTo<SpaceshipController>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<CharacterMediator>().AsSingle();
        Container.BindInterfacesAndSelfTo<AttackComponent>().FromInstance(attackComponent).AsSingle();
        Container.BindInterfacesAndSelfTo<MoveComponent>().FromInstance(moveComponent).AsSingle();
        Container.Bind<Transform>().FromInstance(poolParent).AsSingle();
        Container.Bind<GameUIView>().FromInstance(gameUIView).AsSingle().NonLazy();
        Container.Bind<UIController>().AsSingle().NonLazy();
        Container.Bind<IStorageService>().To<PlayerPrefsStorageService>().AsSingle();
        Container.Bind<IRepository<CharacterStats>>().To<CharacterRepository>().AsSingle();
        Container.Bind<IAnalyticsHandler>().To<FirebaseAnalyticsHandler>().AsSingle();
    }
}
