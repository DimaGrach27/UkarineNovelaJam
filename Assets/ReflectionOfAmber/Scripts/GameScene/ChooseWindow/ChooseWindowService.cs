using System;
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
        }

        public void SetActive(bool isActive)
        {
            _chooseWindowUiView.Visible = isActive;
            IsActive = isActive;
        }
        
        private void OnChooseClick(NextScene chooseScene)
        {
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
            
        }
    }
}