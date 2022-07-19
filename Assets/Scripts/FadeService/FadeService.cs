using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace FadeService
{
    public static class FadeService
    {
        private static readonly FadeUiView FadeUiView;
        
        static FadeService()
        {
            FadeUiView fadeUiView = Resources.Load<FadeUiView>("FadeCanvas");
            FadeUiView = Object.Instantiate(fadeUiView);
            Object.DontDestroyOnLoad(FadeUiView);
            
            FadeUiView.Fade = 0.0f;
            FadeUiView.Visible = false;
        }

        public static void FadeIn(float duration = GlobalConstant.DEFAULT_FADE_DURATION)
        {
            FadeUiView.Fade = 0.0f;
            FadeUiView.Visible = true;
            FadeUiView.CanvasGroup.DOFade(1.0f, duration);
        }
        
        public static async void FadeOut(float duration = GlobalConstant.DEFAULT_FADE_DURATION)
        {
            FadeUiView.Fade = 1.0f;
            FadeUiView.Visible = true;
            FadeUiView.CanvasGroup.DOFade(0.0f, duration);

            await Task.Delay((int)(duration * 1000));
            FadeUiView.Visible = false;
        }

        public static void VisibleFade(bool isVisible) => FadeUiView.Visible = isVisible;
    }
}