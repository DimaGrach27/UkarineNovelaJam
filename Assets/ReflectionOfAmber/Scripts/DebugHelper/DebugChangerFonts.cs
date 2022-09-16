using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.DebugHelper
{
    public class DebugChangerFonts : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] textDialog;
        [SerializeField] private TextMeshProUGUI textName;
        [SerializeField] private ButtonFont[] fontsForDialog;
        [SerializeField] private ButtonFont[] fontsForName;

        private void Awake()
        {
            foreach (var buttonFont in fontsForDialog)
            {
                buttonFont.OnChangeFont += ChangeFontDialog;
                buttonFont.Init();
            }
            
            foreach (var buttonFont in fontsForName)
            {
                buttonFont.OnChangeFont += ChangeFontName;
                buttonFont.Init();
            }
        }

        private void ChangeFontDialog(ButtonFont fontAsset)
        {
            foreach (var textMeshPro in textDialog)
            {
                textMeshPro.font = fontAsset.fontAsset;
                textMeshPro.fontSize = fontAsset.fontSize;
                textMeshPro.lineSpacing = fontAsset.lineSize;
            }
            // textDialog.font = fontAsset;
        }
        
        private void ChangeFontName(ButtonFont fontAsset)
        {
            textName.font = fontAsset.fontAsset;
            textName.fontSize = fontAsset.fontSize;
            textName.lineSpacing = fontAsset.lineSize;
        }
    }

    [Serializable]
    public class ButtonFont
    {
        public event Action<ButtonFont> OnChangeFont; 
        public Button buttonFont;
        public TMP_FontAsset fontAsset;
        public int fontSize;
        public int lineSize = 25;

        public void Init()
        {
            buttonFont.onClick.AddListener(OnClickHandler);
        }
        
        private void OnClickHandler() => OnChangeFont?.Invoke(this);
    }
}