using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ReflectionOfAmber.Scripts.FadeScreen
{
    public class FadeService
    {
        private readonly FadeUiView _fadeUiView;
        private Tween _fadeTween;
        
        public FadeService()
        {
            FadeUiView fadeUiView = Resources.Load<FadeUiView>("FadeCanvas");
            _fadeUiView = Object.Instantiate(fadeUiView);
            Object.DontDestroyOnLoad(_fadeUiView);
            
            _fadeUiView.Fade = 0.0f;
            _fadeUiView.Visible = false;
        }

        public async void FadeIn(float duration = GlobalConstant.DEFAULT_FADE_DURATION, Action onFadeDone = null)
        {
            _fadeUiView.Fade = 0.0f;
            _fadeUiView.Visible = true;

            if (_fadeTween != null) DOTween.Kill(_fadeTween);
            _fadeTween = _fadeUiView.CanvasGroup.DOFade(1.0f, duration).SetEase(Ease.Linear);
            await Task.Delay((int)(duration * 1000));
            onFadeDone?.Invoke();
        }
        
        public async void FadeOut(float duration = GlobalConstant.DEFAULT_FADE_DURATION)
        {
            _fadeUiView.Fade = 1.0f;
            _fadeUiView.Visible = true;
            
            if (_fadeTween != null) DOTween.Kill(_fadeTween);
            _fadeTween = _fadeUiView.CanvasGroup.DOFade(0.0f, duration).SetEase(Ease.Linear);
            
            await Task.Delay((int)(duration * 1000));
            _fadeUiView.Visible = false;
        }

        public void VisibleFade(bool isVisible) => _fadeUiView.Visible = isVisible;
    }
}