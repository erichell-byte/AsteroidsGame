using Analytics;
using Character;
using Config;
using SaveLoad;
using SaveLoad.GameRepository;
using UnityEngine;
using Zenject;

namespace Project.Scripts
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private GameConfiguration gameConfiguration;
        
        public override void InstallBindings()
        {
            Container.Bind<SpaceshipModel>().AsSingle();
            Container.Bind<IGameRepository>().To<PlayerPrefsGameRepository>().AsSingle();
            Container.Bind<ISaveLoader>().To<SpaceshipSaveLoader>().AsSingle();
            Container.Bind<GameConfiguration>().FromInstance(gameConfiguration);
            Container.BindInterfacesAndSelfTo<SaveLoadManager>().AsSingle();
            Container.Bind<IAnalyticsHandler>().To<FirebaseAnalyticsHandler>().AsSingle();
        }
    }
}