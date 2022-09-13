using DG.Tweening;
using ReflectionOfAmber.Scripts.FadeScreen;
using ReflectionOfAmber.Scripts.GameScene.Services;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.LoadScreen;
using ReflectionOfAmber.Scripts.Settings;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ReflectionOfAmber.Scripts.MainMenu
{
    public class MainMenuService : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button startButton;
        [SerializeField] private Button loadButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button exitButton;

        [SerializeField] private CanvasGroup buttonGroup;

        private bool IsGameWasStarted
        {
            get
            {
                int currentProgress = SaveService.GetPart;
                string sceneKey = SaveService.GetScene;

                return currentProgress > 0 || sceneKey != "scene_0_0";
            }
        }

        private AudioSystemService _audioSystemService;
        private ConfirmScreen _confirmScreen;
        private FadeService _fadeService;
        private SceneService _sceneService;
        private LoadScreenService _loadScreenService;
        private SettingsService _settingsService;
        
        [Inject]
        public void Construct(ConfirmScreen confirmScreen, 
            AudioSystemService audioSystemService,
            FadeService fadeService,
            SceneService sceneService,
            LoadScreenService loadScreenService,
            SettingsService settingsService
            )
        {
            _audioSystemService = audioSystemService;
            _confirmScreen = confirmScreen;
            _sceneService = sceneService;
            _fadeService = fadeService;
            _loadScreenService = loadScreenService;
            _settingsService = settingsService;

            _loadScreenService.OnCloseClick += EnableButtonFade;
            _settingsService.OnCloseButtonClick += EnableButtonFade;
        }

        private Tween _fadeTween;
        
        private void Start()
        {
            _fadeService.FadeOut();
            _audioSystemService.StopAllMusic();
            _audioSystemService.StarPlayMusicOnLoop(MusicType.EMBIENT_SLOW);
            
            continueButton.onClick.AddListener(LoadGameScene);
            startButton.onClick.AddListener(StartNewGame);
            loadButton.onClick.AddListener(OpenLoadScreen);
            settingButton.onClick.AddListener(OpenSettingHandler);
            exitButton.onClick.AddListener(Exit);
            
            continueButton.targetGraphic.enabled = IsGameWasStarted;
        }

        private void StartNewGame()
        {
            if (IsGameWasStarted)
            {
                string areYouSure = "Попередній прогрес буде втрачений.\nПродовжити далі?";
                _confirmScreen.Check(ConfirmStart, areYouSure);
                buttonGroup.enabled = true;
                if (_fadeTween != null) DOTween.Kill(_fadeTween);
                
                FadeOutWindow(0.3f);
                return;
            }

            ConfirmStart(true);
        }
        
        private void ConfirmStart(bool isConfirm)
        {
            if(isConfirm)
            {
                SaveService.ResetAllSaves();
                LoadGameScene();
            }
            else
            {
                FadeInWindow(0.5f);
            }
            
        }
        
        private void LoadGameScene()
        {
            _audioSystemService.StopAllMusic();
            _sceneService.LoadGameScene();
        }

        private void Exit()
        {
            string areYouSure = "Ви точно плануєте вийти?";
            _confirmScreen.Check(ConfirmExit, areYouSure);
            buttonGroup.enabled = true;
            FadeOutWindow(0.3f);
        }

        private void ConfirmExit(bool isConfirm)
        {
            if(isConfirm)
                Application.Quit();
            else
                FadeInWindow(0.5f);
        }

        private void OpenLoadScreen()
        {
            buttonGroup.enabled = true;
            _loadScreenService.Open();
            FadeOutWindow(0.3f);
        }

        private void EnableButtonFade()
        {
            FadeInWindow(0.5f);
        }
        
        private void FadeInWindow(float duration)
        {
            if (_fadeTween != null) DOTween.Kill(_fadeTween);
            
            buttonGroup.interactable = true;
            buttonGroup.blocksRaycasts = true;
            _fadeTween = buttonGroup.DOFade(1.0f, duration);
        }
        
        private void FadeOutWindow(float duration)
        {
            if (_fadeTween != null) DOTween.Kill(_fadeTween);
            
            buttonGroup.interactable = false;
            buttonGroup.blocksRaycasts = false;
            _fadeTween = buttonGroup.DOFade(0.0f, duration);
        }

        private void OpenSettingHandler()
        {
            buttonGroup.enabled = true;
            _settingsService.Open();
            FadeOutWindow(0.3f);
        }
    }
}