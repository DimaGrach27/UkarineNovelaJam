using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace ReflectionOfAmber.Scripts
{
    public class SceneService
    {
        [Inject]
        public SceneService(CoroutineHelper coroutineHelper)
        {
            _coroutineHelper = coroutineHelper;
        }

        private readonly CoroutineHelper _coroutineHelper;
        
        private Coroutine _loadCoroutine;

        public void LoadEndGame()
        {
            if (_loadCoroutine != null) return;

            SaveService.ResetAllSaves();

            _loadCoroutine = _coroutineHelper.StartCoroutine(LoadRoutine());
        }
        
        private IEnumerator LoadRoutine()
        {
            FadeService.FadeService.FadeIn();
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene("EndScene");
        }
    }
}