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
            Container.Bind<GameConfiguration>().FromInstance(_gameConfiguration).AsSingle();
            Container.Bind<ISerializer>().To<NewtonsoftSerializer>().AsSingle();
            Container.Bind<IKeysProvider>().To<MapKeysProvider>().AsSingle();
            Container.Bind<SaveSystemFacade>().AsSingle();

            Container.Bind<IDataStorage>().WithId(StorageId.Local).To<PlayerPrefsDataStorage>().AsSingle();
            Container.Bind<IDataStorage>().WithId(StorageId.Cloud).To<CloudSaveDataStorage>().AsSingle();
            
            Container.Bind<ISaveSystem>().To<SaveSystem>().AsSingle().NonLazy();
            
            Container.Bind<IAnalyticsHandler>().To<FirebaseAnalyticsHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnityAdsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AddressablesBootstrap>().AsSingle();
            Container.BindInterfacesAndSelfTo<FirebaseConfigProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnityPurchaseService>().AsSingle();
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
        }
    }
    
    public enum StorageId { Local, Cloud }
}