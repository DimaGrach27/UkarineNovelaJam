using System.Collections;
using DG.Tweening;
using ReflectionOfAmber.Scripts.FadeScreen;
using ReflectionOfAmber.Scripts.GameScene.Services;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace ReflectionOfAmber.Scripts.EndSceneDemo
{
    public class EndSceneDemoService : MonoBehaviour, IPointerClickHandler, IInputListener
    {
        [SerializeField] 
        private CanvasGroup tapToExit;
        [SerializeField] 
        private CanvasGroup groupImages;

        private bool m_IsReadyToTap;
        private bool m_LoadRunning;
        
        private AudioSystemService m_AudioSystemService;
        private FadeService m_FadeService;
        private SceneService m_SceneService;
        private InputService m_inputService;
        
        [Inject]
        public void Construct(
            AudioSystemService audioSystemService,
            FadeService fadeService,
            SceneService sceneService,
            InputService inputService
            )
        {
            m_AudioSystemService = audioSystemService;
            m_FadeService = fadeService;
            m_SceneService = sceneService;
            m_inputService = inputService;
            
            m_inputService.AddListener(this);
        }

        private void Awake()
        {
            m_AudioSystemService.StopAllMusic();
            
            tapToExit.alpha = 0.0f;
            groupImages.alpha = 0.0f;
            m_FadeService.VisibleFade(false);
        }

        private void Start()
        {
            StartCoroutine(DelayWait());
        }

        private void OnDestroy()
        {
            m_inputService.RemoveListener(this);
        }
        
        private IEnumerator DelayWait()
        {
            yield return new WaitForSeconds(1.0f);
            
            groupImages.DOFade(1.0f, 2.0f);
            
            yield return new WaitForSeconds(2.5f);
            
            tapToExit.DOFade(1.0f, 0.75f);
            m_IsReadyToTap = true;

            ShouldReceiveInput = true;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if(!m_IsReadyToTap || m_LoadRunning)
            {
                return;
            }

            LoadMainMenu();
        }

        private void LoadMainMenu()
        {
            m_LoadRunning = true;
            m_SceneService.LoadMainMenuScene();
        }

        public void OnInputAction(InputActionEnum inputActionEnum)
        {
            if(!m_IsReadyToTap || m_LoadRunning)
            {
                return;
            }
            
            LoadMainMenu();
        }

        public bool ShouldReceiveInput { get; set; }
    }
}