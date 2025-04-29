using System;
using DG.Tweening;
using ReflectionOfAmber.Scripts.FadeScreen;
using ReflectionOfAmber.Scripts.GameScene.Services;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
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
#if GAME_DEMO
                if (sceneKey == GlobalConstant.LAST_DEMO_SCENE_KEY)
                {
                    return false;
                }
#endif
                return currentProgress > 0 || sceneKey != "scene_0_0";
            }
        }

        private AudioSystemService m_audioSystemService;
        private ConfirmScreen m_confirmScreen;
        private FadeService m_fadeService;
        private SceneService m_sceneService;
        private LoadScreenService m_loadScreenService;
        private SettingsService m_settingsService;
        
        [Inject]
        public void Construct(ConfirmScreen confirmScreen, 
            AudioSystemService audioSystemService,
            FadeService fadeService,
            SceneService sceneService,
            LoadScreenService loadScreenService,
            SettingsService settingsService
            )
        {
            m_audioSystemService = audioSystemService;
            m_confirmScreen = confirmScreen;
            m_sceneService = sceneService;
            m_fadeService = fadeService;
            m_loadScreenService = loadScreenService;
            m_settingsService = settingsService;

            m_loadScreenService.OnCloseClick += EnableButtonFade;
            m_settingsService.OnCloseButtonClick += EnableButtonFade;
        }

        private Tween _fadeTween;
        
        private void Start()
        {
            m_fadeService.FadeOut();
            m_audioSystemService.StopAllMusic();
            m_audioSystemService.StarPlayMusicOnLoop(MusicType.EMBIENT_SLOW);
            
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
                m_confirmScreen.Check(ConfirmStart, TranslatorKeys.CONFIRM_NEW_GAME);
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
            m_audioSystemService.StopAllMusic();
            m_sceneService.LoadGameScene();
        }

        private void Exit()
        {
            m_confirmScreen.Check(ConfirmExit, TranslatorKeys.CONFIRM_EXIT);
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
            m_loadScreenService.Open();
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
            m_settingsService.Open();
            FadeOutWindow(0.3f);
        }

        private void OnDestroy()
        {
            m_loadScreenService.OnCloseClick -= EnableButtonFade;
            m_settingsService.OnCloseButtonClick -= EnableButtonFade;
        }
    }
}