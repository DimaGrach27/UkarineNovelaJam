using System.Collections;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using TMPro;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene
{
    public class InfoDescription : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private ClickHelper clickHelper;
        
        public ScreenPartsServiceFacade ScreenPartsServiceFacade { get; set; }
        
        private List<string> _texts;
        private Coroutine _delayWait;
        
        private bool _isReadyToClick;
        private bool _isTyping;
        
        private void Awake()
        {
            clickHelper.OnClick += OnPointerClick;
        }

        public void SetInfoDescription(List<string> textsShow)
        {
            _texts = new List<string>();
            
            foreach (var textShow in textsShow)
            {
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
    }
}