using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.PreInitScene;
using UnityEngine.SceneManagement;
using Zenject;

namespace ReflectionOfAmber.Scripts.ProjectInstallers
{
    public class EntryPoint : IInitializable
    {
        private readonly GameIniter m_gameIniter;
        private readonly LoadingScreenView m_LoadingScreenView;

        [Inject]
        private EntryPoint(GameIniter gameIniter, LoadingScreenView loadingScreenView)
        {
            m_gameIniter = gameIniter;
        }
        
        public void Initialize()
        {
            m_gameIniter.OnInitEnded += InitEndedHandler;
            m_gameIniter.Initialize();
        }

        private void InitEndedHandler()
        {
            m_gameIniter.OnInitEnded -= InitEndedHandler;
            
            // if (SaveService.BrightnessStatus)
            // {
            LoadMineMenu();
            // }
            // else
            // {
            // Object.Destroy(m_LoadingScreenView.gameObject);
            // }
        }
        
        private void LoadMineMenu()
        {
            SaveService.Init();
            SaveService.BrightnessStatus = true;
            SceneManager.LoadScene("MainMenu");
        }
    }
}