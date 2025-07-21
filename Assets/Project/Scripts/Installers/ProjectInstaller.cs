using Analytics;
using AssetsLoader;
using Character;
using Config;
using GameAdvertisement;
using Purchasing;
using SaveLoad;
using Systems;
using UnityEngine;
using Zenject;

namespace Project.Scripts
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private GameConfigurationSO gameConfigurationSO;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameCycle>().AsSingle();
            Container.Bind<SpaceshipModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnityCloudSaveRepository>().AsSingle();
            Container.Bind<ILocalRepository>().To<PlayerPrefsRepository>().AsSingle();
            Container.Bind<ISaveLoader>().To<SpaceshipDataSaveLoader>().AsSingle();
            Container.Bind<ISaveLoader>().To<PurchasedDataSaveLoader>().AsSingle();
            Container.Bind<GameConfigurationSO>().FromInstance(gameConfigurationSO).AsSingle();
            Container.BindInterfacesAndSelfTo<SaveLoadManager>().AsSingle();
            Container.Bind<IAnalyticsHandler>().To<FirebaseAnalyticsHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnityAdsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AddressablesBootstrap>().AsSingle();
            Container.Bind<IConfigProvider>().To<FirebaseConfigProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameConfigApplier>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UnityPurchaseService>().AsSingle();
        }
    }
}