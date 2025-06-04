using SaveLoad;
using Zenject;

namespace Project.Scripts
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IDataContainer<CharacterStats>>().To<CharacterDataContainer>().AsSingle();
        }
    }
}