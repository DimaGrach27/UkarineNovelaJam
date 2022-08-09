using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenText
{
    public class ScreenTextUiView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI mainText;
        [SerializeField] private Scrollbar scrollbar;
        
        private CanvasGroup _canvasGroup;
        public CanvasGroup CanvasGroup
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
            set => gameObject.SetActive(value);
        }

        public string Text
        {
            set
            {
                mainText.text = value;
                scrollbar.value = 1.0f;
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
    }
}