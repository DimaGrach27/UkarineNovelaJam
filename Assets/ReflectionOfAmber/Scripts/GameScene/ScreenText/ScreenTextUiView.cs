using ReflectionOfAmber.Scripts.GlobalProject;
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
        
        private CanvasGroup m_CanvasGroup;
        public CanvasGroup CanvasGroup => m_CanvasGroup ??= GetComponent<CanvasGroup>();

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

        public int CharacterCount => mainText.textInfo.characterCount;

        public int MaxVisibleCharacters
        {
            set => mainText.maxVisibleCharacters = value;
        }

        public int PrepareTyping(string text, int previousTextLength)
        {
            // Parse complete rich-text tags before revealing any characters.
            mainText.maxVisibleCharacters = 0;
            Text = text;
            mainText.ForceMeshUpdate(ignoreActiveState: true);

            int visibleCharacters = 0;
            while (visibleCharacters < CharacterCount &&
                   mainText.textInfo.characterInfo[visibleCharacters].index < previousTextLength)
            {
                visibleCharacters++;
            }

            mainText.maxVisibleCharacters = visibleCharacters;
            return visibleCharacters;
        }

        void Update()
        {
            MoveScrollbar();
        }

        private void MoveScrollbar()
        {
            scrollbar.ScrollByMouseWheel();
        }
    }
}
