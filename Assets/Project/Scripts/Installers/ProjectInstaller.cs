using Analytics;
using AssetsLoader;
using Character;
using Config;
using GameAdvertisement;
using SaveLoad;
using SaveLoad.GameRepository;
using Systems;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Project.Scripts
{
    public class ProjectInstaller : MonoInstaller
    {
        [FormerlySerializedAs("gameConfiguration")]
        [SerializeField] private GameConfigurationSO gameConfigurationSo;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameCycle>().AsSingle();
            Container.Bind<SpaceshipModel>().AsSingle();
            Container.Bind<IGameRepository>().To<PlayerPrefsGameRepository>().AsSingle();
            Container.Bind<ISaveLoader>().To<SaveLoader>().AsSingle();
            Container.Bind<GameConfigurationSO>().FromInstance(gameConfigurationSo).AsSingle();
            Container.BindInterfacesAndSelfTo<SaveLoadManager>().AsSingle();
            Container.Bind<IAnalyticsHandler>().To<FirebaseAnalyticsHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnityAdsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AddressablesBootstrap>().AsSingle();
            Container.Bind<IConfigProvider>().To<FirebaseConfigProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameConfigApplier>().AsSingle().NonLazy();
        }
    }
}