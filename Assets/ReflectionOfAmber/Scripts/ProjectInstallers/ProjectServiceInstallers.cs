using ReflectionOfAmber.Scripts.GameScene.Services;
using ReflectionOfAmber.Scripts.MainMenu;
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
            ServicesInstallers();

            SceneManager.LoadScene("MainMenu");
        }
        
        private void ServicesInstallers()
        {
            Container.Bind<CoroutineHelper>().FromInstance(coroutineHelper).AsSingle().NonLazy();
            Container.Bind<AudioSystemService>().FromInstance(audioSystemService).AsSingle().NonLazy();
            Container.Bind<ConfirmScreen>().FromInstance(confirmScreen).AsSingle().NonLazy();
            Container.Bind<SceneService>().AsSingle().NonLazy();
        }
    }
}