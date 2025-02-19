using System.Collections;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using ReflectionOfAmber.Scripts.Input;
using TMPro;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene
{
    public class InfoDescription : MonoBehaviour, IInputListener
    {
        [SerializeField] 
        private TextMeshProUGUI text;
        
        [SerializeField] 
        private ClickHelper clickHelper;
        
        [SerializeField] 
        private GameObject m_cameraIcon;
        
        public ScreenPartsServiceFacade ScreenPartsServiceFacade { get; set; }
        
        private List<string> _texts;
        private Coroutine _delayWait;
        
        private bool _isReadyToClick;
        private bool _isTyping;

        private InputService m_inputService;
        
        [Inject]
        public void Construct(InputService inputService)
        {
            Debug.Log($"Construct: {gameObject.name}");
            m_inputService = inputService;
        }
        
        private void Awake()
        {
            clickHelper.OnClick += OnPointerClick;
        }

        public void SetInfoDescription(TranslatorKeys[] textsShow, bool showCameraIcon)
        {
            m_cameraIcon.SetActive(showCameraIcon);
            m_inputService.ForceRedirectInput(this);
            _texts = new List<string>();
            
            foreach (var textShowKey in textsShow)
            {
                string textShow = TranslatorParser.GetText(textShowKey.ToString());
                _texts.Add(textShow);
            }
            
            if (_texts.Count > 0)
            {
                if(_delayWait != null)
                    StopCoroutine(_delayWait);
                _delayWait = StartCoroutine(TypingRoutine());
            }
        }
        
        private void OnPointerClick()
        {
            if (_isTyping)
            {
                if(_delayWait != null)
                    StopCoroutine(_delayWait);
                
                text.text = _texts[0];
                _texts.RemoveAt(0);
                _isReadyToClick = true;
                _isTyping = false;
                return;
            }
            
            if(!_isReadyToClick) return;
            
            if (_texts.Count > 0)
            {
                if(_delayWait != null)
                    StopCoroutine(_delayWait);
                _delayWait = StartCoroutine(TypingRoutine());
                return;
            }
            
            m_inputService.RemoveForceRedirected(this);
            ScreenPartsServiceFacade.PlatNextPart();
            Destroy(gameObject);
        }
        
        
        private IEnumerator TypingRoutine()
        {
            _isReadyToClick = false;
            _isTyping = true;
            string resultText = "";

            foreach (char symbol in _texts[0])
            {
                resultText += symbol;
                text.text = resultText;
                // yield return new WaitForSeconds(GlobalConstant.TYPING_SPEED);
            }
            
            yield return null;

            _texts.RemoveAt(0);
            _isReadyToClick = true;
            _isTyping = false;
        }

        public void OnInputAction(InputAction inputAction)
        {
            if (inputAction == InputAction.SPACE)
            {
                OnPointerClick();
            }
        }
        
        public class Factory : PlaceholderFactory<InfoDescription> { }
    }
}