using System;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Misc;
using ReflectionOfAmber.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views
{
    public class NoteWindowScreenButton : MonoBehaviour
    {
        [SerializeField] private NoteWindowScreensEnum noteWindowScreensEnum;
        [SerializeField] private float m_fillOffAmount;
        [SerializeField] private Image m_image;

        public NoteWindowScreensEnum NoteWindowScreensEnum => noteWindowScreensEnum;
        
        public event Action<NoteWindowScreensEnum> OnClickButton;

        private void Awake()
        {
            ButtonExt buttonExt = GetComponent<ButtonExt>();
            buttonExt.onClick.AddListener(OnClickButtonHandler);
            buttonExt.SetSelectedOnHover(false);
        }

        private void OnClickButtonHandler() => OnClickButton?.Invoke(noteWindowScreensEnum);

        public void IsActive(bool value)
        {
            m_image.fillAmount = value ? 1.0f : m_fillOffAmount;
        }
    }
}