using System;
using System.Threading.Tasks;
using DG.Tweening;
using ReflectionOfAmber.Scripts.GlobalProject;
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

        public void FadeIn(float duration = GlobalConstant.DEFAULT_FADE_DURATION, Action onFadeDone = null)
        {
            _fadeUiView.Fade = 0.0f;
            _fadeUiView.Visible = true;

            if (_fadeTween != null)
            {
                DOTween.Kill(_fadeTween);
            }
            
            _fadeTween = _fadeUiView.CanvasGroup.DOFade(1.0f, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                onFadeDone?.Invoke();
            });
        }
        
        public void FadeOut(float duration = GlobalConstant.DEFAULT_FADE_DURATION, Action onFadeDone = null)
        {
            _fadeUiView.Fade = 1.0f;
            _fadeUiView.Visible = true;
            
            if (_fadeTween != null)
            {
                DOTween.Kill(_fadeTween);
            }
            _fadeTween = _fadeUiView.CanvasGroup.DOFade(0.0f, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                _fadeUiView.Visible = false;
                onFadeDone?.Invoke();
            });
        }

        public void VisibleFade(bool isVisible) => _fadeUiView.Visible = isVisible;
    }
}