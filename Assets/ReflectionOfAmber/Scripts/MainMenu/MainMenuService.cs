using System.Collections;
using DG.Tweening;
using ReflectionOfAmber.Scripts.GameScene.Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace ReflectionOfAmber.Scripts.MainMenu
{
    public class MainMenuService : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button startButton;
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
        
        [Inject]
        public void Construct(ConfirmScreen confirmScreen, AudioSystemService audioSystemService)
        {
            _audioSystemService = audioSystemService;
            _confirmScreen = confirmScreen;
        }
        
        private void Start()
        {
            FadeService.FadeService.FadeOut();
            _audioSystemService.StopAllMusic();
            _audioSystemService.StarPlayMusicOnLoop(MusicType.EMBIENT_SLOW);
            
            continueButton.onClick.AddListener(LoadGameScene);
            startButton.onClick.AddListener(StartNewGame);
            exitButton.onClick.AddListener(Exit);
            
            continueButton.targetGraphic.enabled = IsGameWasStarted;
            
            if(GameModel.GameWasInit) return;
            
            GameModel.Init();
        }

        private void StartNewGame()
        {
            if (IsGameWasStarted)
            {
                string areYouSure = "Попередній прогрес буде втрачений.\nПродовжити далі?";
                _confirmScreen.Check(ConfirmStart, areYouSure);
                buttonGroup.enabled = true;
                buttonGroup.DOFade(0.0f, 0.3f);
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
                buttonGroup.DOFade(1.0f, 0.5f);
            }
            
        }
        
        private void LoadGameScene()
        {
            _audioSystemService.StopAllMusic();
            StartCoroutine(LoadGameSceneRoutine());
        }

        IEnumerator LoadGameSceneRoutine()
        {
            float durationFade = GlobalConstant.DEFAULT_FADE_DURATION;

            FadeService.FadeService.FadeIn(durationFade);
            
            yield return new WaitForSeconds(durationFade);
            SceneManager.LoadScene("MainScene");
        }
        
        private void Exit()
        {
            string areYouSure = "Ви точно плануєте вийти?";
            _confirmScreen.Check(ConfirmExit, areYouSure);
            buttonGroup.enabled = true;
            buttonGroup.DOFade(0.0f, 0.3f);
        }

        private void ConfirmExit(bool isConfirm)
        {
            if(isConfirm)
                Application.Quit();
            else
                buttonGroup.DOFade(1.0f, 0.5f);
        }
    }
}