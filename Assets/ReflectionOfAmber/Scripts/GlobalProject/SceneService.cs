using System.Collections;
using ReflectionOfAmber.Scripts.FadeScreen;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace ReflectionOfAmber.Scripts.GlobalProject
{
    public class SceneService
    {
        [Inject]
        public SceneService(CoroutineHelper coroutineHelper,
            FadeService fadeService)
        {
            _coroutineHelper = coroutineHelper;
            _fadeService = fadeService;
        }

        private readonly CoroutineHelper _coroutineHelper;
        private readonly FadeService _fadeService;
        
        private Coroutine _loadCoroutine;

        public void LoadEndGame()
        {
            if (_loadCoroutine != null) return;

            SaveService.ResetAllSaves();

            _loadCoroutine = _coroutineHelper.StartCoroutine(LoadRoutine(Scenes.EndScene));
        }

        public void LoadGameScene()
        {
            if (_loadCoroutine != null) return;
            _loadCoroutine = _coroutineHelper.StartCoroutine(LoadRoutine(Scenes.MainScene));
        }
        
        public void LoadMainMenuScene()
        {
            if (_loadCoroutine != null) return;
            _loadCoroutine = _coroutineHelper.StartCoroutine(LoadRoutine(Scenes.MainMenuScene));
        }
        
        private IEnumerator LoadRoutine(string sceneName)
        {
            _fadeService.FadeIn();
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene(sceneName);
        }
    }

    public struct Scenes
    {
        public const string MainMenuScene = "MainMenu";
        public const string MainScene = "MainScene";
        public const string EndScene = "EndScene";
    } 
}