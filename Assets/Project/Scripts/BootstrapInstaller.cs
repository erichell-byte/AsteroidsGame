using Zenject;
using Systems;

public class BootstrapInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<ApplicationManager>().AsSingle();
    }
}