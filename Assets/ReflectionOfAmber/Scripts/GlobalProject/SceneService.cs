using ReflectionOfAmber.Scripts.FadeScreen;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace ReflectionOfAmber.Scripts.GlobalProject
{
    public class SceneService
    {
        [Inject]
        public SceneService(
            FadeService fadeService)
        {
            _fadeService = fadeService;
        }

        private readonly FadeService _fadeService;
        
        private Coroutine _loadCoroutine;

        public void LoadEndGame()
        {
            SaveService.ResetAllSaves();
            LoadScene(Scenes.EndScene);
        }

        public void LoadGameScene()
        {
            LoadScene(Scenes.MainScene);
        }
        
        public void LoadMainMenuScene()
        {
            LoadScene(Scenes.MainMenuScene);
        }
        
        private void LoadScene(string sceneName)
        {
            float duration = 1.5f;
            _fadeService.FadeIn(duration, () =>
            {
                SceneManager.LoadScene(sceneName);
            });
        }
    }

    public struct Scenes
    {
        public const string MainMenuScene = "MainMenu";
        public const string MainScene = "MainScene";
        public const string EndScene = "EndScene";
    } 
}