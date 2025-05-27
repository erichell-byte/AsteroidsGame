using Config;
using GameSystem;
using Systems;
using UnityEngine;
using Utils;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameCycle gameCycle;
    [SerializeField] private GameEventsController gameEventsController;
    [SerializeField] private GameConfiguration gameConfiguration;
    
    public override void InstallBindings()
    {
        Container.Bind<GameCycle>().FromInstance(gameCycle).NonLazy();
        Container.Bind<GameEventsController>().FromInstance(gameEventsController).NonLazy();
        Container.BindFactory<Timer, Timer.Factory>().AsTransient();
        Container.Bind<GameConfiguration>().FromInstance(gameConfiguration);
        Container.BindInterfacesAndSelfTo<TimersController>().AsSingle().NonLazy();

    }
}
