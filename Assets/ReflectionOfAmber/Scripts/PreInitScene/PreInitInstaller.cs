using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.PreInitScene
{
    public class PreInitInstaller : MonoInstaller<PreInitInstaller>
    {
        [SerializeField] private LoadingScreenView loadingScreenView;
        
        public override void InstallBindings()
        {
            Container.Bind<LoadingScreenView>().FromInstance(loadingScreenView).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameIniter>().AsSingle().NonLazy();
        }
    }
}