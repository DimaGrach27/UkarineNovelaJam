using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene.ScreenText
{
    public class ScreenServiceUiView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI mainText;
        [SerializeField] private Scrollbar scrollbar;
        
        private CanvasGroup _canvasGroup;
        private CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                    _canvasGroup = GetComponent<CanvasGroup>();
                return _canvasGroup;
            }
        }
        
        public bool Visible
        {
            get => Math.Abs(CanvasGroup.alpha - 1.0f) < 0.05f;
            
            set
            {
                bool currentState = Visible;
                
                if (!currentState && value)
                {
                    // DissolveIn();
                }
                
                if (currentState && !value)
                {
                    // DissolveOut();
                }
                
                gameObject.SetActive(value);
            }
        }

        public string Text
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Visible = false;
                    return;
                }
                
                mainText.text = value;
                scrollbar.value = 1.0f;

                Visible = true;
            }
        }
        
        public string Name
        {
            set
            {
                nameText.text = value;
                nameText.enabled = !string.IsNullOrEmpty(value);
            }
        }
        
        private void DissolveIn(float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION)
        {
            CanvasGroup.alpha = 0.0f;
            CanvasGroup.DOFade(1.0f, duration);
        }
        
        private void DissolveOut(float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION)
        {
            CanvasGroup.alpha = 1.0f;
            CanvasGroup.DOFade(0.0f, duration);
        }
    }
}