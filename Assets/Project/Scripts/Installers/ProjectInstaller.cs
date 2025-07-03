using Analytics;
using AssetsLoader;
using Character;
using Config;
using GameAdvertisement;
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
            Container.Bind<GameConfiguration>().FromInstance(gameConfiguration).AsSingle();
            Container.BindInterfacesAndSelfTo<SaveLoadManager>().AsSingle();
            Container.Bind<IAnalyticsHandler>().To<FirebaseAnalyticsHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnityAdsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AddressablesBootstrap>().AsSingle();
            Container.Bind<IConfigProvider>().To<FirebaseConfigProvider>().AsSingle();
            Container.Bind<IInitializable>().To<GameConfigApplier>().AsSingle().NonLazy();
        }
    }
}