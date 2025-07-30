using Zenject;
using Systems;

namespace Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameBootstrapper>().AsSingle();
        }
    }
}