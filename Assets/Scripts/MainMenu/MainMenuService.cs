﻿using System.Collections;
using DG.Tweening;
using GameScene.Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
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
        
        private void Start()
        {
            FadeService.FadeService.FadeOut();
            
            
            continueButton.onClick.AddListener(LoadGameScene);
            startButton.onClick.AddListener(StartNewGame);
            exitButton.onClick.AddListener(Exit);
            
            continueButton.targetGraphic.enabled = IsGameWasStarted;
            AudioSystemService.Inst.StopAllMusic();
            
            GameModel.Init();
        }

        private void StartNewGame()
        {
            if (IsGameWasStarted)
            {
                string areYouSure = "Попередній прогрес буде втрачений.\nПродовжити далі?";
                ConfirmScreen.Ins.Check(ConfirmStart, areYouSure);
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
            ConfirmScreen.Ins.Check(ConfirmExit, areYouSure);
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