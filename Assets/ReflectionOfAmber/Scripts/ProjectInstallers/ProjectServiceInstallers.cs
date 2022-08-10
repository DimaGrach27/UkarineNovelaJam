using ReflectionOfAmber.Scripts.GameScene.Services;
using ReflectionOfAmber.Scripts.MainMenu;
using ReflectionOfAmber.Scripts.FadeScreen;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace ReflectionOfAmber.Scripts.ProjectInstallers
{
    public class ProjectServiceInstallers : MonoInstaller<ProjectServiceInstallers>
    {
        [SerializeField] private CoroutineHelper coroutineHelper;
        [SerializeField] private AudioSystemService audioSystemService;
        [SerializeField] private ConfirmScreen confirmScreen;
        
        public override void InstallBindings()
        {
            GameModel.Init();
            
            ServicesInstallers();

            SceneManager.LoadScene("MainMenu");
        }
        
        private void ServicesInstallers()
        {
            Container.Bind<ScreenPartsServiceFacade>().AsSingle().NonLazy();
            
            Container.Bind<CoroutineHelper>().FromInstance(coroutineHelper).AsSingle().NonLazy();
            Container.Bind<AudioSystemService>().FromInstance(audioSystemService).AsSingle().NonLazy();
            Container.Bind<ConfirmScreen>().FromInstance(confirmScreen).AsSingle().NonLazy();
            Container.Bind<SceneService>().AsSingle().NonLazy();
            Container.Bind<FadeService>().AsSingle().NonLazy();
        }
    }
}