using ReflectionOfAmber.Scripts.Analytic;
using ReflectionOfAmber.Scripts.Authenticator;
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
        [Header("DEBUG:")]
        [SerializeField] 
        private DebugHelperService debugHelperService;
#endif
        public override void InstallBindings()
        {
            ServicesInstallers();
            // GameModel.Init();

#if !GAME_FINAL
            DebugInstallers();
#endif
        }
        
        private void ServicesInstallers()
        {
            Container.BindInterfacesAndSelfTo<UserUnityService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameResourcesService>().AsSingle().NonLazy();
#if STEAM_GAME
            Container.BindInterfacesAndSelfTo<SteamService>().AsSingle().NonLazy();
#endif
            Container.BindInterfacesAndSelfTo<AuthenticatorService>().AsSingle().NonLazy();
#if ANALYTIC_ENABLED          
            Container.BindInterfacesAndSelfTo<AnalyticService>().AsSingle().NonLazy();
#endif
            Container.BindInterfacesAndSelfTo<GameModel>().AsSingle().NonLazy();

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

#if STEAM_GAME
            SaveService.SteamService = Container.Resolve<SteamService>();
#endif
        }
        
#if !GAME_FINAL
        private void DebugInstallers()
        {
            DebugHelperService debugHelperSer = Instantiate(debugHelperService);
            Container.Bind<DebugHelperService>().FromInstance(debugHelperSer).AsSingle().NonLazy();
            Container.Inject(debugHelperSer.debugOpenSceneService);
        }
#endif
    }
}