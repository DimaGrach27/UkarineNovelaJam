using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameScene
{
    public class InfoDescription : MonoBehaviour
    {
        [SerializeField, TextArea(1, 5)] private List<string> texts;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private ClickHelper clickHelper;
        
        private Coroutine _delayWait;
        private bool _isReadyToClick;
        private bool _isTyping;
        private void Awake()
        {
            if (texts.Count > 0)
            {
                if(_delayWait != null)
                    StopCoroutine(_delayWait);
                _delayWait = StartCoroutine(TypingRoutine());
            }

            clickHelper.OnClick += OnPointerClick;
        }

        private void OnPointerClick()
        {
            if (_isTyping)
            {
                if(_delayWait != null)
                    StopCoroutine(_delayWait);
                
                text.text = texts[0];
                texts.RemoveAt(0);
                _isReadyToClick = true;
                _isTyping = false;
                return;
            }
            
            if(!_isReadyToClick) return;
            
            if (texts.Count > 0)
            {
                if(_delayWait != null)
                    StopCoroutine(_delayWait);
                _delayWait = StartCoroutine(TypingRoutine());
                return;
            }
            
            Destroy(gameObject);
        }
        
        
        private IEnumerator TypingRoutine()
        {
            _isReadyToClick = false;
            _isTyping = true;
            string resultText = "";

            foreach (char symbol in texts[0])
            {
                resultText += symbol;
                text.text = resultText;
                yield return new WaitForSeconds(GlobalConstant.TYPING_SPEED);
            }

            texts.RemoveAt(0);
            _isReadyToClick = true;
            _isTyping = false;
        }
    }
}