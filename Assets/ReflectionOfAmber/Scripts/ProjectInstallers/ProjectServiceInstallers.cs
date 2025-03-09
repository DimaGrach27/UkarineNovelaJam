using ReflectionOfAmber.Scripts.DebugHelper;
using ReflectionOfAmber.Scripts.GameScene.Services;
using ReflectionOfAmber.Scripts.FadeScreen;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using ReflectionOfAmber.Scripts.Input;
using ReflectionOfAmber.Scripts.Settings;
using ReflectionOfAmber.Scripts.Steam;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.ProjectInstallers
{
    public class ProjectServiceInstallers : MonoInstaller<ProjectServiceInstallers>
    {
        [SerializeField] private CoroutineHelper coroutineHelper;
        [SerializeField] private AudioSystemService audioSystemService;
        [SerializeField] private ConfirmScreen confirmScreen;
        [SerializeField] private GlobalBrightnessService globalBrightnessService;
        [SerializeField] private SettingsService settingsService;

#if !GAME_FINAL
        //DEBUG
        [SerializeField] private DebugHelperService debugHelperService;
#endif


        public override void InstallBindings()
        {
            GameModel.Init();
            
            ServicesInstallers();
        }
        
        private void ServicesInstallers()
        {
            Container.BindInterfacesAndSelfTo<SteamService>().AsSingle().NonLazy();

            Container.Bind<ScreenPartsServiceFacade>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<SettingsService>().FromInstance(settingsService).AsSingle().NonLazy();
            Container.Bind<CoroutineHelper>().FromInstance(coroutineHelper).AsSingle().NonLazy();
            Container.Bind<AudioSystemService>().FromInstance(audioSystemService).AsSingle().NonLazy();
            Container.Bind<ConfirmScreen>().FromInstance(confirmScreen).AsSingle().NonLazy();
            Container.Bind<GlobalBrightnessService>().FromInstance(globalBrightnessService).AsSingle().NonLazy();
            Container.Bind<SceneService>().AsSingle().NonLazy();
            Container.Bind<FadeService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<TranslatorService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InputService>().AsSingle().NonLazy();

            SaveService.SteamService = Container.Resolve<SteamService>();
            DebugInstallers();
        }
        
        private void DebugInstallers()
        {
#if !GAME_FINAL
            Container.Bind<DebugHelperService>().FromInstance(debugHelperService).AsSingle().NonLazy();
#endif
        }
    }
}