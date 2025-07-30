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

namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private GameConfiguration _gameConfiguration;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameCycle>().AsSingle();
            Container.Bind<SpaceshipModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnityCloudSaveStorage>().AsSingle();
            Container.Bind<ILocalStorage>().To<PlayerPrefsStorage>().AsSingle();
            Container.Bind<ISaveLoader>().To<SpaceshipDataSaveLoader>().AsSingle();
            Container.Bind<ISaveLoader>().To<PurchasedDataSaveLoader>().AsSingle();
            Container.Bind<GameConfiguration>().FromInstance(_gameConfiguration).AsSingle();
            Container.BindInterfacesAndSelfTo<GameSaveService>().AsSingle();
            Container.Bind<IAnalyticsHandler>().To<FirebaseAnalyticsHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnityAdsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AddressablesBootstrap>().AsSingle();
            Container.BindInterfacesAndSelfTo<FirebaseConfigProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnityPurchaseService>().AsSingle();
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
        }
    }
}