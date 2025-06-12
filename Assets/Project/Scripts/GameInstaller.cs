using Components;
using GameSystem;
using Analytics;
using Character;
using Enemies;
using Systems;
using UI;
using UnityEngine;
using Utils;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private AttackComponent attackComponent;
    [SerializeField] private MoveComponent moveComponent;
    [SerializeField] private CollisionComponent collisionComponent;
    [SerializeField] private Transform poolParent;
    [SerializeField] private GameUIView gameUIView;
    [SerializeField] private EnemiesManager enemiesManager;
    
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameCycle>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameEventsController>().AsSingle();
        Container.Bind<CollisionComponent>().FromInstance(collisionComponent);
        Container.BindFactory<Timer, Timer.Factory>().FromNew();
        Container.BindInterfacesAndSelfTo<TimersController>().AsSingle();
        Container.BindInterfacesAndSelfTo<AttackComponent>().FromInstance(attackComponent).AsSingle();
        Container.BindInterfacesAndSelfTo<MoveComponent>().FromInstance(moveComponent).AsSingle();
        Container.BindInterfacesAndSelfTo<SpaceshipController>().AsSingle();
        Container.Bind<Transform>().FromInstance(poolParent).AsSingle();
        Container.Bind<GameUIView>().FromInstance(gameUIView).AsSingle();
        Container.Bind<UIController>().AsSingle().NonLazy();
        Container.Bind<IAnalyticsHandler>().To<FirebaseAnalyticsHandler>().AsSingle();
        Container.Bind<EnemiesManager>().FromInstance(enemiesManager).AsSingle();
        Container.BindInterfacesAndSelfTo<AnalyticsMediator>().AsSingle();
        
    }
}
