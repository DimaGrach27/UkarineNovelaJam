using System.Collections;
using DG.Tweening;
using ReflectionOfAmber.Scripts.GameScene.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Zenject;

namespace ReflectionOfAmber.Scripts.EndScene
{
    public class EndSceneService : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private CanvasGroup tapToExit;
        [SerializeField] private CanvasGroup groupImages;

        private bool _isReadyToTap;

        private Coroutine _delayLoad;
        private AudioSystemService _audioSystemService;

        [Inject]
        public void Construct(AudioSystemService audioSystemService)
        {
            _audioSystemService = audioSystemService;
        }

        private void Awake()
        {
            _audioSystemService.StopAllMusic();
            
            tapToExit.alpha = 0.0f;
            groupImages.alpha = 0.0f;
            FadeService.FadeService.VisibleFade(false);
        }

        private void Start()
        {
            StartCoroutine(DelayWait());
        }

        private IEnumerator DelayWait()
        {
            yield return new WaitForSeconds(1.0f);
            
            groupImages.DOFade(1.0f, 2.0f);
            
            yield return new WaitForSeconds(2.5f);
            
            tapToExit.DOFade(1.0f, 0.75f);
            _isReadyToTap = true;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if(!_isReadyToTap || _delayLoad != null) return;

            _delayLoad = StartCoroutine(DelayLoadMainMenu());
        }

        private IEnumerator DelayLoadMainMenu()
        {
            float duration = 2.0f;
            ReflectionOfAmber.Scripts.FadeService.FadeService.FadeIn(duration);
            yield return new WaitForSeconds(duration);
            SaveService.ResetAllSaves();
            SceneManager.LoadScene("MainMenu");
        }
    }
}