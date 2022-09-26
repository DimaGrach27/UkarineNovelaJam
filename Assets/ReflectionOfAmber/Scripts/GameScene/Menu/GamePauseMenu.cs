using System.Collections;
using DG.Tweening;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.Menu
{
    public class GamePauseMenu : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button exitToMenuButton;
        
        private ConfirmScreen _confirmScreen;
        private SceneService _sceneService;
        
        private CanvasGroup _canvasGroup;
        
        private Coroutine _routine;
        private Coroutine _delayLoad;

        [Inject]
        public void Construct(
            ConfirmScreen confirmScreen,
            SceneService sceneService
            )
        {
            _confirmScreen = confirmScreen;
            _sceneService = sceneService;
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
            FadeOut();
        }
        
        private void Open(CallKeyType type)
        {
            if(type != CallKeyType.GAME_PAUSE_MENU) return;
            FadeIn();
        }
        
        private void ClickExit()
        {
            FadeOut();
            _confirmScreen.Check(ConfirmExit, TranslatorKeys.CONFIRM_EXIT);
        }
        
        private void ConfirmExit(bool isExit)
        {
            if(!isExit)
            {
                FadeIn();
                return;
            }
            
            GameModel.IsGamePlaying = false;
            _sceneService.LoadMainMenuScene();
        }

        private void FadeOut()
        {
            if(_routine != null)
                StopCoroutine(_routine);

            _routine = StartCoroutine(FadeOutWindow());
        }

        private void FadeIn()
        {
            if(_routine != null)
                StopCoroutine(_routine);

            _routine = StartCoroutine(FadeInWindow());
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