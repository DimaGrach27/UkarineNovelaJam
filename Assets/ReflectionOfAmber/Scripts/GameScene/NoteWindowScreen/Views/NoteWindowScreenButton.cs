using System;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Misc;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views
{
    public class NoteWindowScreenButton : MonoBehaviour
    {
        [SerializeField] private NoteWindowScreensEnum noteWindowScreensEnum;

        public NoteWindowScreensEnum NoteWindowScreensEnum => noteWindowScreensEnum;
        
        public event Action<NoteWindowScreensEnum> OnClickButton;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnClickButtonHandler);
        }

        private void OnClickButtonHandler() => OnClickButton?.Invoke(noteWindowScreensEnum);
    }
}