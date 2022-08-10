using System;
using System.Collections;
using DG.Tweening;
using ReflectionOfAmber.Scripts.FadeScreen;
using ReflectionOfAmber.Scripts.MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.Menu
{
    public class GamePauseMenu : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button exitToMenuButton;
        
        private FadeService _fadeService;
        private ConfirmScreen _confirmScreen;
        
        private CanvasGroup _canvasGroup;
        
        private Coroutine _routine;
        private Coroutine _delayLoad;

        [Inject]
        public void Construct(ConfirmScreen confirmScreen,
            FadeService fadeService)
        {
            _confirmScreen = confirmScreen;
            _fadeService = fadeService;
        }
        
        private void Awake()
        {
            exitToMenuButton.onClick.AddListener(ClickExit);
            continueButton.onClick.AddListener(Close);
            
            _canvasGroup = GetComponent<CanvasGroup>();
            
            GlobalEvent.OnCallType += Open;
            
            _canvasGroup.interactable = false;
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
        }

        private void Close()
        {
            if(_routine != null)
                StopCoroutine(_routine);

            _routine = StartCoroutine(FadeOutWindow());
        }
        
        private void Open(CallKeyType type)
        {
            if(type != CallKeyType.GAME_PAUSE_MENU) return;
            
            if(_routine != null)
                StopCoroutine(_routine);
            
            _routine = StartCoroutine(FadeInWindow());
        }
        
        private void ClickExit()
        {
            _confirmScreen.Check(ConfirmExit, "Ви точно бажаєте вийти?");
        }
        
        private void ConfirmExit(bool isExit)
        {
            if(!isExit) return;
            if(_delayLoad != null) return;
            
            _delayLoad = StartCoroutine(DelayLoadMainMenu());
        }
        
        private IEnumerator DelayLoadMainMenu()
        {
            float duration = 2.0f;
            _fadeService.FadeIn(duration);
            yield return new WaitForSeconds(duration);
            SceneManager.LoadScene("MainMenu");
        }
        
        private IEnumerator FadeInWindow()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION;
            _canvasGroup.DOFade(1.0f, duration);
            yield return null;
        }
        
        private IEnumerator FadeOutWindow()
        {
            _canvasGroup.interactable = false;
            float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION;
            _canvasGroup.DOFade(0.0f, duration);
            yield return new WaitForSeconds(duration);
            _canvasGroup.blocksRaycasts = false;
        }

        private void OnDestroy()
        {
            GlobalEvent.OnCallType -= Open;
        }
    }
}