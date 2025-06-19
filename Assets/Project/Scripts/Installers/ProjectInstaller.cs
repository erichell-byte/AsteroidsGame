using Analytics;
using Character;
using Config;
using SaveLoad;
using SaveLoad.GameRepository;
using Systems;
using UnityEngine;
using Zenject;

namespace Project.Scripts
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private GameConfiguration gameConfiguration;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameCycle>().AsSingle();
            Container.Bind<SpaceshipModel>().AsSingle();
            Container.Bind<IGameRepository>().To<PlayerPrefsGameRepository>().AsSingle();
            Container.Bind<ISaveLoader>().To<SaveLoader>().AsSingle();
            Container.Bind<GameConfiguration>().FromInstance(gameConfiguration);
            Container.BindInterfacesAndSelfTo<SaveLoadManager>().AsSingle();
            Container.Bind<IAnalyticsHandler>().To<FirebaseAnalyticsHandler>().AsSingle();
        }
    }
}