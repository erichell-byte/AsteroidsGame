using UI;
using UnityEngine;
using Zenject;

namespace Installers
{
	public class MenuSceneInstaller : MonoInstaller
	{
		[SerializeField] private MenuView _menuView;

		public override void InstallBindings()
		{
			Container.Bind<MenuView>().FromInstance(_menuView).AsSingle();
			Container.Bind<MenuController>().AsSingle().NonLazy();
		}
	}
}