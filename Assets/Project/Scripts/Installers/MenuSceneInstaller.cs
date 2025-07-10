using Project.Scripts.UI;
using UnityEngine;
using Zenject;

namespace Project.Scripts
{
    public class MenuSceneInstaller : MonoInstaller
    {
        [SerializeField] private MenuView menuView;

        public override void InstallBindings()
        {
            Container.Bind<MenuView>().FromInstance(menuView).AsSingle();
            Container.Bind<MenuController>().AsSingle().NonLazy();
        }
    }
}