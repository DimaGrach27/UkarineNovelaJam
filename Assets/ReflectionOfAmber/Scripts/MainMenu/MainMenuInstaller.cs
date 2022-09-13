using ReflectionOfAmber.Scripts.LoadScreen;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.MainMenu
{
    public class MainMenuInstaller : MonoInstaller<MainMenuInstaller>
    {
        [SerializeField] private LoadScreenView loadScreenView;
        
        public override void InstallBindings()
        {
            Container.Bind<LoadScreenView>().FromInstance(loadScreenView).AsSingle().NonLazy();
            Container.Bind<LoadScreenService>().AsSingle().NonLazy();
        }
    }
}