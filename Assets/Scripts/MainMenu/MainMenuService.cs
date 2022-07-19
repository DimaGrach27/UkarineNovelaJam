using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuService : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button startButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;

        [SerializeField] private ConfirmScreen confirmScreen;

        private bool IsGameWasStarted
        {
            get
            {
                int currentProgress = SaveService.GetPart();
                string sceneKey = SaveService.GetScene();

                return currentProgress > 0 || sceneKey != "screen_scene_0";
            }
        }
        
        private void Start()
        {
            continueButton.interactable = IsGameWasStarted;
            
            continueButton.onClick.AddListener(LoadGameScene);
            startButton.onClick.AddListener(StartNewGame);
            exitButton.onClick.AddListener(Exit);
        }

        private void StartNewGame()
        {
            if (IsGameWasStarted)
            {
                string areYouSure = "Якщо ви почнете спочатку, то прогрессу буде втрачений. Ви впевнені?";
                confirmScreen.Check(ConfirmStart, areYouSure);
                
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
        }
        
        private void LoadGameScene()
        {
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
            confirmScreen.Check(ConfirmExit, areYouSure);
        }

        private void ConfirmExit(bool isConfirm)
        {
            if(isConfirm)
                Application.Quit();
        }
    }
}