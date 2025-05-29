using System;
using System.Collections;
using DG.Tweening;
using ReflectionOfAmber.Scripts.FadeScreen;
using ReflectionOfAmber.Scripts.GameScene.Services;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace ReflectionOfAmber.Scripts.EndScene
{
    public class EndSceneService : MonoBehaviour, IPointerClickHandler, IInputListener
    {
        [SerializeField] private CanvasGroup tapToExit;
        [SerializeField] private CanvasGroup groupImages;

        private bool _isReadyToTap;
        private bool m_loadRunning;
        
        private AudioSystemService _audioSystemService;
        private FadeService _fadeService;
        private SceneService m_SceneService;
        private InputService m_inputService;

        [Inject]
        public void Construct(
            AudioSystemService audioSystemService,
            FadeService fadeService,
            SceneService sceneService,
            InputService inputService)
        {
            _audioSystemService = audioSystemService;
            _fadeService = fadeService;
            m_SceneService = sceneService;
            m_inputService = inputService;
            
            m_inputService.AddListener(this);
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
            _isReadyToTap = true;

            ShouldReceiveInput = true;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if(!_isReadyToTap || m_loadRunning)
            {
                return;
            }

            LoadMainMenu();
        }

        private void LoadMainMenu()
        {
            m_loadRunning = true;
            SaveService.ResetAllSaves();
            m_SceneService.LoadMainMenuScene();
        }

        public void OnInputAction(InputAction inputAction)
        {
            if(!_isReadyToTap || m_loadRunning)
            {
                return;
            }
            
            LoadMainMenu();
        }

        public bool ShouldReceiveInput { get; set; }
    }
}