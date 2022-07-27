using System;
using System.Collections;
using DG.Tweening;
using GameScene.Services;
using UnityEngine;

namespace GameScene.ScreenText
{
    public class ScreenTextService
    {
        public event Action OnEndTyping;
        
        public ScreenTextService(Transform uiTransform, UiClickHandler uiClickHandler)
        {
            _screenTextUiView = uiTransform.GetComponentInChildren<ScreenTextUiView>();
            uiClickHandler.OnClick += EndTyping;
        }
     
        private readonly ScreenTextUiView _screenTextUiView;
        
        private bool _isTextEnable = true;
        private bool _isTyping;

        private string _endText;

        private Coroutine _typingCoroutine;
        private Coroutine _dissolveCoroutine;

        public void ShowText()
        {
            if(_isTextEnable) return;
            
            if(_dissolveCoroutine != null)
                CoroutineHelper.Inst.StopCoroutine(_dissolveCoroutine);
            
            _isTextEnable = true;
            _screenTextUiView.Visible = true;
            DissolveIn();
        }
        
        public void SetText(string name, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                _screenTextUiView.Visible = false;
                return;
            }

            _screenTextUiView.Name = name;
            _endText = text;
            
            if(_typingCoroutine != null)
                CoroutineHelper.Inst.StopCoroutine(_typingCoroutine);
            
            _typingCoroutine = CoroutineHelper.Inst.StartCoroutine(TypingRoutine(text));
            
            ShowText();
        }
        
        public void HideText()
        {
            if(!_isTextEnable) return;
            
            if(_dissolveCoroutine != null)
                CoroutineHelper.Inst.StopCoroutine(_dissolveCoroutine);
            
            _dissolveCoroutine = CoroutineHelper.Inst.StartCoroutine(DissolveOut());
            
            _isTextEnable = false;
        }

        private IEnumerator TypingRoutine(string text)
        {
            _screenTextUiView.CanvasGroup.blocksRaycasts = false;
            _isTyping = true;
            string resultText = "";

            foreach (char symbol in text)
            {
                resultText += symbol;
                _screenTextUiView.Text = resultText;
                yield return new WaitForSeconds(GlobalConstant.TYPING_SPEED);
            }

            EndTyping();
        }

        private void DissolveIn(float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION)
        {
            _screenTextUiView.CanvasGroup.alpha = 0.0f;
            _screenTextUiView.CanvasGroup.DOFade(1.0f, duration);
        }
        
        private IEnumerator DissolveOut(float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION)
        {
            _screenTextUiView.CanvasGroup.alpha = 1.0f;
            _screenTextUiView.CanvasGroup.DOFade(0.0f, duration);
            yield return new WaitForSeconds(duration);
            _screenTextUiView.Visible = false;
        }

        private void EndTyping()
        {
            if(!_isTyping)
            {
                OnEndTyping?.Invoke();
                return;
            }
            
            if(_typingCoroutine != null)
                CoroutineHelper.Inst.StopCoroutine(_typingCoroutine);
            
            _isTyping = false;
            _screenTextUiView.CanvasGroup.blocksRaycasts = true;
            if(_endText != null) _screenTextUiView.Text = _endText;
            
            _endText = null;
        }
    }
}