using System;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Misc;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.UI;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views
{
    public class NoteWindowScreenPopup : UIScreenBase
    {
        public event Action<NoteWindowScreensEnum> OnSelectWindowClick;
        
        [SerializeField] private NoteWindowScreenButton[] buttons;
        [SerializeField] private NoteWindowScreenBgView noteWindowScreenBgView;
        
        private Dictionary<NoteWindowScreensEnum, NoteWindowScreenButton> _buttonsNoteMap;

        public event Action OnClose;
        
        private void Awake()
        {
            if (_buttonsNoteMap == null)
            {
                _buttonsNoteMap = new();
                foreach (var button in buttons)
                {
                    _buttonsNoteMap.Add(button.NoteWindowScreensEnum, button);
                    button.OnClickButton += OnSelectWindowHandler;
                }
            }
        }

        protected override void PreOpen()
        {
            gameObject.SetActive(true);
        }

        public void OpenWithClose()
        {
            Open();
            if (_buttonsNoteMap == null)
            {
                _buttonsNoteMap = new();
                foreach (var button in buttons)
                {
                    _buttonsNoteMap.Add(button.NoteWindowScreensEnum, button);
                    button.OnClickButton += OnSelectWindowHandler;
                }
            }

            if (SaveService.GetStatusValue(StatusEnum.CHOOSE_WAS_PICK))
            {
                _buttonsNoteMap[NoteWindowScreensEnum.INVESTIGATION_SCREEN].gameObject.SetActive(false);
            }
            
            OnSelectWindowHandler(NoteWindowScreensEnum.MAIN_SCREEN);
        }
        
        public void OpenWithoutCanClose()
        {
            Open();
            GlobalEvent.HideCanvas();

            foreach (var button in buttons)
            {
                button.gameObject.SetActive(false);
            }

            OnSelectWindowHandler(NoteWindowScreensEnum.INVESTIGATION_SCREEN);
        }
        
        public void Hide()
        {
            OnClose?.Invoke();
        }
        
        private void OnSelectWindowHandler(NoteWindowScreensEnum noteWindowScreensEnum)
        {
            bool isFirstPage = noteWindowScreensEnum == NoteWindowScreensEnum.MAIN_SCREEN;

            foreach (var keyValue in _buttonsNoteMap)
            {
                keyValue.Value.IsActive(keyValue.Key == noteWindowScreensEnum);
            }
            noteWindowScreenBgView.FirstPage = isFirstPage;
            // noteWindowScreenBgView.SetLastElement();
            // _buttonsNoteMap[noteWindowScreensEnum].transform.SetAsLastSibling();
            
            OnSelectWindowClick?.Invoke(noteWindowScreensEnum);
        }
    }
}