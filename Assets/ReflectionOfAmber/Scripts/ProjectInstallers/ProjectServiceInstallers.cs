using ReflectionOfAmber.Scripts.GameScene.Services;
using ReflectionOfAmber.Scripts.FadeScreen;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using ReflectionOfAmber.Scripts.Settings;
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
        
        public override void InstallBindings()
        {
            GameModel.Init();
            
            ServicesInstallers();
        }
        
        private void ServicesInstallers()
        {
            Container.Bind<ScreenPartsServiceFacade>().AsSingle().NonLazy();
            
            Container.Bind<SettingsService>().FromInstance(settingsService).AsSingle().NonLazy();
            Container.Bind<CoroutineHelper>().FromInstance(coroutineHelper).AsSingle().NonLazy();
            Container.Bind<AudioSystemService>().FromInstance(audioSystemService).AsSingle().NonLazy();
            Container.Bind<ConfirmScreen>().FromInstance(confirmScreen).AsSingle().NonLazy();
            Container.Bind<GlobalBrightnessService>().FromInstance(globalBrightnessService).AsSingle().NonLazy();
            Container.Bind<SceneService>().AsSingle().NonLazy();
            Container.Bind<FadeService>().AsSingle().NonLazy();
            Container.Bind<TranslatorParser>().AsSingle().NonLazy();
        }
    }
}