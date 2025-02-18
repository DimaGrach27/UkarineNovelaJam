using System;
using System.Collections;
using DG.Tweening;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GameScene.Services;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.Input;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenText
{
    public class ScreenTextService : IInputListener, IDisposable
    {
        public event Action OnEndTyping;
        
        [Inject]
        public ScreenTextService(GamePlayCanvas gamePlayCanvas, 
            UiClickHandler uiClickHandler, 
            ScreenPartNextDialogButton screenPartNextDialogButton, 
            CoroutineHelper coroutineHelper,
            InputService inputService)
        {
            _coroutineHelper = coroutineHelper;
            _screenTextUiView = gamePlayCanvas.GetComponentInChildren<ScreenTextUiView>();
            m_inputService = inputService;
            uiClickHandler.OnClick += EndTyping;
            screenPartNextDialogButton.OnClickButton += EndTyping;
            m_inputService.AddListener(this);
        }
     
        private readonly ScreenTextUiView _screenTextUiView;
        private readonly CoroutineHelper _coroutineHelper;
        private readonly InputService m_inputService;
        
        private bool _isTextEnable = true;
        private bool _isTyping;
        private bool _isEndOfText = true;

        private string _endText;
        private string _prevText;

        private Coroutine _typingCoroutine;
        private Coroutine _dissolveCoroutine;
        
        public void SetText(string name, string text, bool endOfText)
        {
            _endText = "";
            _isEndOfText = endOfText;
            
            if (string.IsNullOrEmpty(text))
            {
                HideText();
                return;
            }
            
            _screenTextUiView.Name = name;

            _endText = _prevText + text;

            if(_typingCoroutine != null)
                _coroutineHelper.StopCoroutine(_typingCoroutine);
            
            _typingCoroutine = _coroutineHelper.StartCoroutine(TypingRoutine(text));
            
            ShowText();
        }
        
        private void ShowText()
        {
            if(_isTextEnable) return;
            
            if(_dissolveCoroutine != null)
                _coroutineHelper.StopCoroutine(_dissolveCoroutine);
            
            _isTextEnable = true;
            _screenTextUiView.Visible = true;
            DissolveIn();
        }
        
        public void HideText()
        {
            if(!_isTextEnable) return;
            
            if(_dissolveCoroutine != null)
                _coroutineHelper.StopCoroutine(_dissolveCoroutine);
            
            _dissolveCoroutine = _coroutineHelper.StartCoroutine(DissolveOut());
            
            _isTextEnable = false;
        }

        private IEnumerator TypingRoutine(string text)
        {
            _screenTextUiView.CanvasGroup.blocksRaycasts = false;
            _isTyping = true;
            string resultText = _prevText;

            foreach (char symbol in text)
            {
                resultText += symbol;
                _screenTextUiView.Text = resultText;
                yield return new WaitForSeconds(GameModel.TYPING_SPEED);
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
                _coroutineHelper.StopCoroutine(_typingCoroutine);
            
            _isTyping = false;
            _screenTextUiView.CanvasGroup.blocksRaycasts = true;
            if(_endText != null) _screenTextUiView.Text = _endText;
            
            _prevText = _isEndOfText ? "" : _endText;
            _endText = null;
        }

        public void OnInputAction(InputAction inputAction)
        {
            if (inputAction == InputAction.SPACE)
            {
                EndTyping();
            }
        }

        public void Dispose()
        {
            if(_typingCoroutine != null && _coroutineHelper != null)
            {
                _coroutineHelper.StopCoroutine(_typingCoroutine);
            }
            
            if(_dissolveCoroutine != null && _coroutineHelper != null)
            {
                _coroutineHelper.StopCoroutine(_dissolveCoroutine);
            }
            
            m_inputService.RemoveListener(this);
        }
    }
}