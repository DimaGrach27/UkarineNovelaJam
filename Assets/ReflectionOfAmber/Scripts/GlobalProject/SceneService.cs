using ReflectionOfAmber.Scripts.FadeScreen;
using ReflectionOfAmber.Scripts.Input;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace ReflectionOfAmber.Scripts.GlobalProject
{
    public class SceneService
    {
        [Inject]
        public SceneService(
            FadeService fadeService,
            InputService inputService)
        {
            m_FadeService = fadeService;
            m_InputService = inputService;
        }

        private readonly FadeService m_FadeService;
        private readonly InputService m_InputService;

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
#if GAME_DEMO
        public void EndDemoScene()
        {
            LoadScene(Scenes.EndDemoScene);
        }
#endif
        private void LoadScene(string sceneName)
        {
            m_InputService.ForceBlockInput(true);
            float duration = 1.5f;
            m_FadeService.FadeIn(duration, () =>
            {
                m_InputService.ForceBlockInput(false);
                SceneManager.LoadScene(sceneName);
            });
        }
    }

    public struct Scenes
    {
        public const string MainMenuScene = "MainMenu";
        public const string MainScene = "MainScene";
        public const string EndScene = "EndScene";
#if GAME_DEMO
        public const string EndDemoScene = "EndSceneDemo";
#endif
    } 
}