using System;
using DG.Tweening;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using ReflectionOfAmber.Scripts.Input;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ReflectionOfAmber.Scripts.GlobalProject
{
    public class ConfirmScreen : MonoBehaviour, IInputListener
    {
        [SerializeField] private TextMeshProUGUI textDescription;
        
        [SerializeField] private Button confirm;
        [SerializeField] private Button notConfirm;

        private CanvasGroup _canvasGroup;
        private Action<bool> _currentAction;

        private Tween _tween;
        private InputService m_inputService;

        private void Awake()
        {
            confirm.onClick.AddListener(Confirm);
            notConfirm.onClick.AddListener(NotConfirm);
            
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        [Inject]
        public void Construct(InputService inputService)
        {
            m_inputService = inputService;
        }

        public void Check(Action<bool> onSelectAction, TranslatorKeys translatorKey)
        {
            if (_tween != null)
                DOTween.Kill(_tween);
            
            _canvasGroup.blocksRaycasts = true;
            _tween = _canvasGroup.DOFade(1.0f, 0.5f);
            
            textDescription.text = TranslatorParser.GetText(translatorKey);
            
            _currentAction = onSelectAction;
            
            m_inputService.ForceRedirectInput(this);
        }

        private void Confirm()
        {
            _currentAction?.Invoke(true);
            Close();
        }
        
        private void NotConfirm()
        {
            _currentAction?.Invoke(false);
            Close();
        }

        private void Close()
        {
            if (_tween != null)
                DOTween.Kill(_tween);

            _canvasGroup.blocksRaycasts = false;
            _tween = _canvasGroup.DOFade(0.0f, 0.5f);
            
            _currentAction = null;
            
            m_inputService.RemoveForceRedirected(this);
        }

        public void OnInputAction(InputAction inputAction)
        {
            if (inputAction == InputAction.PAUSE)
            {
                NotConfirm();
            }
        }
    }
}