using System;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.Analytic;
using System.Collections;
using DG.Tweening;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.ChooseWindow
{
    public class ChooseWindowService : IDisposable
    {
        private readonly ChooseWindowUiView _chooseWindowUiView;
        private readonly CoroutineHelper _coroutineHelper;

        public event Action<NextScene> OnChoose;

        private Coroutine _coroutine;
        private float choiceStarted;
        private bool cameraChoice;
        private bool acceptingChoice;
        
        public bool IsActive { get; private set; }

        [Inject]
        public ChooseWindowService(GamePlayCanvas gamePlayCanvas, CoroutineHelper coroutineHelper)
        {
            _chooseWindowUiView = gamePlayCanvas.GetComponentInChildren<ChooseWindowUiView>();
            _coroutineHelper = coroutineHelper;
            
            _chooseWindowUiView.OnChoose += OnChooseClick;
        }

        public void SetChooses(NextScene[] nextScenes, string textChoose, bool isCameraAction)
        {
            if(_coroutine !=null) _coroutineHelper.StopCoroutine(_coroutine);
            _coroutine = _coroutineHelper.StartCoroutine(FadeInWindow());
            
            _chooseWindowUiView.InitButtons(nextScenes, isCameraAction);
            _chooseWindowUiView.SetChooseText(textChoose);
            choiceStarted = DemoAnalytics.ActiveSeconds;
            cameraChoice = isCameraAction;
            acceptingChoice = true;
            string scene = SaveService.GetScene;
            DemoAnalytics.Track("ChoiceShown", $"ChoiceShown:{scene}:{isCameraAction}", data:
                new Dictionary<string, object> { { "SceneID", scene }, { "OptionCount", nextScenes.Length }, { "Camera", isCameraAction } });
            foreach (var option in nextScenes)
                DemoAnalytics.Track("ChoiceOptionShown", $"ChoiceOptionShown:{scene}:{option.Scene.SceneKey}", data:
                    new Dictionary<string, object> { { "SceneID", scene }, { "OptionID", option.Scene.SceneKey }, { "Camera", isCameraAction } });
        }

        public void SetActive(bool isActive)
        {
            _chooseWindowUiView.Visible = isActive;
            IsActive = isActive;
            if (!isActive) acceptingChoice = false;
        }
        
        private void OnChooseClick(NextScene chooseScene)
        {
            if (!acceptingChoice) return;
            acceptingChoice = false;
            float seconds = DemoAnalytics.ActiveSeconds - choiceStarted;
            string scene = SaveService.GetScene;
            DemoAnalytics.Track("ChoiceSelected", $"ChoiceSelected:{scene}:{chooseScene.Scene.SceneKey}", seconds,
                new Dictionary<string, object> { { "SceneID", scene }, { "OptionID", chooseScene.Scene.SceneKey },
                    { "DecisionSeconds", seconds }, { "Camera", cameraChoice } });
            if(_coroutine !=null) _coroutineHelper.StopCoroutine(_coroutine);
            _coroutine = _coroutineHelper.StartCoroutine(FadeOutWindow());
            
            OnChoose?.Invoke(chooseScene);
        }

        private IEnumerator FadeInWindow()
        {
            SetActive(true);
            float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION;
            _chooseWindowUiView.CanvasGroup.DOFade(1.0f, duration);
            yield return null;
        }
        
        private IEnumerator FadeOutWindow()
        {
            float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION;
            _chooseWindowUiView.CanvasGroup.DOFade(0.0f, duration);
            yield return new WaitForSeconds(duration);
            SetActive(false);
        }

        public void Dispose()
        {
            _chooseWindowUiView.OnChoose -= OnChooseClick;
        }
    }
}