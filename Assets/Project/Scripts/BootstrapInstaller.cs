using Zenject;
using SaveLoad;

public class BootstrapInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IStorageService>().To<PlayerPrefsStorageService>().AsSingle();
        Container.Bind<IRepository<CharacterStats>>().To<CharacterRepository>().AsSingle();
        
        var repository = Container.Resolve<IRepository<CharacterStats>>();
        var dataContainer = Container.Resolve<IDataContainer<CharacterStats>>();
        
        if (repository.Load(out CharacterStats stats))
        {
            dataContainer.SetData(stats);
        }

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Game", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}