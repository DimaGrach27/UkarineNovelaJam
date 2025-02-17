using System;
using System.Collections;
using ReflectionOfAmber.Scripts.GameScene;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIScreenBase : MonoBehaviour
    {
        protected CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                    _canvasGroup = GetComponent<CanvasGroup>();
                return _canvasGroup;
            }
        }
        private CanvasGroup _canvasGroup;

        private Coroutine m_fadeRoutine;

        public void Open()
        {
            GlobalEvent.HideCanvas();

            PreOpen();
            FadeInWindow();
        }

        public void Close()
        {
            GlobalEvent.ShowCanvas();

            PreClose();
            FadeOutWindow();
        }

        protected virtual void PreOpen() { }
        protected virtual void AfterOpen() { }
        
        protected virtual void PreClose() { }
        protected virtual void AfterClose() { }
        
        private void FadeInWindow()
        {
            if (m_fadeRoutine != null)
            {
                StopCoroutine(m_fadeRoutine);
            }

            m_fadeRoutine = StartCoroutine(FadeInRoutine());
        }
        
        private void FadeOutWindow()
        {
            if (m_fadeRoutine != null)
            {
                StopCoroutine(m_fadeRoutine);
            }

            m_fadeRoutine = StartCoroutine(FadeOutRoutine());
        }
        
        private IEnumerator FadeOutRoutine()
        {
            CanvasGroup.alpha = 1.0f;
            float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION;
            while (duration > 0)
            {
                float currentAlpha = duration / GlobalConstant.ANIMATION_DISSOLVE_DURATION;
                currentAlpha = Math.Max(currentAlpha, 0);
                CanvasGroup.alpha = currentAlpha;
                duration -= Time.deltaTime;
                yield return null;
            }

            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
            CanvasGroup.alpha = 0.0f;
            m_fadeRoutine = null;

            AfterClose();
        }
        
        private IEnumerator FadeInRoutine()
        {
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
            CanvasGroup.alpha = 0.0f;
            
            float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION;
            while (duration > 0)
            {
                float currentAlpha = duration / GlobalConstant.ANIMATION_DISSOLVE_DURATION;
                currentAlpha = 1.0f - currentAlpha;
                
                currentAlpha = Math.Min(currentAlpha, 1);
                CanvasGroup.alpha = currentAlpha;
                duration -= Time.deltaTime;
                yield return null;
            }

            CanvasGroup.alpha = 1.0f;
            m_fadeRoutine = null;

            AfterOpen();
        }
    }
}