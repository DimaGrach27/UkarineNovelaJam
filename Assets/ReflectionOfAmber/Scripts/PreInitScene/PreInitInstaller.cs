using ReflectionOfAmber.Scripts.DebugHelper;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.PreInitScene
{
    public class PreInitInstaller : MonoInstaller<PreInitInstaller>
    {
        [SerializeField] 
        private LoadingScreenView loadingScreenView;
        
#if !GAME_FINAL
        //DEBUG
        [SerializeField] 
        private DebugHelperService debugHelperService;
#endif
        public override void InstallBindings()
        {
            Container.Bind<LoadingScreenView>().FromInstance(loadingScreenView).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameIniter>().AsSingle().NonLazy();
            
#if !GAME_FINAL
            DebugInstallers();
#endif
        }
        
#if !GAME_FINAL
        private void DebugInstallers()
        {
            Container.Bind<DebugHelperService>().FromInstance(debugHelperService).AsSingle().NonLazy();
        }
#endif
    }
}