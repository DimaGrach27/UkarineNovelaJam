using System.Collections;
using DG.Tweening;
using ReflectionOfAmber.Scripts.FadeScreen;
using ReflectionOfAmber.Scripts.GameScene.Services;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace ReflectionOfAmber.Scripts.EndScene
{
    public class EndSceneService : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private CanvasGroup tapToExit;
        [SerializeField] private CanvasGroup groupImages;

        private bool _isReadyToTap;
        private bool m_loadRunning;

        // private Coroutine _delayLoad;
        private AudioSystemService _audioSystemService;
        private FadeService _fadeService;
        private SceneService m_SceneService;

        [Inject]
        public void Construct(
            AudioSystemService audioSystemService,
            FadeService fadeService,
            SceneService sceneService)
        {
            _audioSystemService = audioSystemService;
            _fadeService = fadeService;
            m_SceneService = sceneService;
        }

        private void Awake()
        {
            _audioSystemService.StopAllMusic();
            
            tapToExit.alpha = 0.0f;
            groupImages.alpha = 0.0f;
            _fadeService.VisibleFade(false);
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
            if(!_isReadyToTap || m_loadRunning)
            {
                return;
            }
            float duration = 2.0f;

            _fadeService.FadeIn(duration, LoadMainMenu);
        }

        private void LoadMainMenu()
        {
            SaveService.ResetAllSaves();
            m_SceneService.LoadMainMenuScene();
        }
    }
}