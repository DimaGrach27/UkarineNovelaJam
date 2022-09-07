using System;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Misc;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views
{
    public class NoteWindowScreenPopup : MonoBehaviour
    {
        public event Action<NoteWindowScreensEnum> OnSelectWindowClick;
        
        [SerializeField] private NoteWindowScreenBgView noteWindowScreenBgView;
        
        private readonly Dictionary<NoteWindowScreensEnum, NoteWindowScreenButton> _buttonsNoteMap = new();
        
        private void Awake()
        {
            foreach (var button in GetComponentsInChildren<NoteWindowScreenButton>())
            {
                _buttonsNoteMap.Add(button.NoteWindowScreensEnum, button);
                button.OnClickButton += OnSelectWindowHandler;
            }
        }

        public void Open()
        {
            OnSelectWindowHandler(NoteWindowScreensEnum.MAIN_SCREEN);
        }
        
        private void OnSelectWindowHandler(NoteWindowScreensEnum noteWindowScreensEnum)
        {
            bool isFirstPage = noteWindowScreensEnum == NoteWindowScreensEnum.MAIN_SCREEN;
            
            noteWindowScreenBgView.FirstPage = isFirstPage;
            noteWindowScreenBgView.SetLastElement();
            _buttonsNoteMap[noteWindowScreensEnum].transform.SetAsLastSibling();
            
            OnSelectWindowClick?.Invoke(noteWindowScreensEnum);
        }
    }
}