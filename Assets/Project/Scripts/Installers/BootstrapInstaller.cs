using Systems;
using Zenject;

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