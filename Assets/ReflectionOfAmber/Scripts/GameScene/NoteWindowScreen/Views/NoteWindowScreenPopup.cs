using System;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Misc;
using ReflectionOfAmber.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views
{
    public class NoteWindowScreenPopup : UIScreenBase
    {
        public event Action<NoteWindowScreensEnum> OnSelectWindowClick;
        
        [SerializeField] 
        private NoteWindowScreenButton[] buttons;
        [SerializeField] 
        private NoteWindowScreenBgView noteWindowScreenBgView;
        
        private Dictionary<NoteWindowScreensEnum, NoteWindowScreenButton> _buttonsNoteMap;

        public event Action OnClose;
        
        private void Awake()
        {
            InitButtons();
        }

        protected override void PreOpen()
        {
            gameObject.SetActive(true);
        }

        public void OpenWithClose()
        {
            Open();
            InitButtons();

            foreach (var button in buttons)
            {
                if (button.NoteWindowScreensEnum == NoteWindowScreensEnum.MAIN_SCREEN)
                {
                    FocusUIManager.Instance.JumpSelectionToObject(button.GetComponent<ButtonExt>());
                }
            }
            
#if !GAME_DEMO
            bool canOpenInvestigationScreen = !SaveService.GetStatusValue(StatusEnum.CHOOSE_WAS_PICK);
            _buttonsNoteMap[NoteWindowScreensEnum.INVESTIGATION_SCREEN].gameObject.SetActive(canOpenInvestigationScreen);
#endif
            OnSelectWindowHandler(NoteWindowScreensEnum.MAIN_SCREEN);
        }

        private void InitButtons()
        {
            if (_buttonsNoteMap != null)
            {
                return;
            }

            _buttonsNoteMap = new();
            foreach (var button in buttons)
            {
#if GAME_DEMO
                if (button.NoteWindowScreensEnum == NoteWindowScreensEnum.INVESTIGATION_SCREEN)
                {
                    foreach (var buttonInert in buttons)
                    {
                        if (buttonInert.NoteWindowScreensEnum == NoteWindowScreensEnum.SETTINGS_SCREEN)
                        {
                            ButtonExt btn = buttonInert.GetComponent<ButtonExt>();
                            if (btn)
                            {
                                Navigation nav = btn.navigation;
                                nav.selectOnDown = null;
                                btn.navigation = nav;
                            }
                        }
                    }
                    button.gameObject.SetActive(false);
                    continue;
                }
#endif
                _buttonsNoteMap.Add(button.NoteWindowScreensEnum, button);
                button.OnClickButton += OnSelectWindowHandler;
            }
        }
        
#if !GAME_DEMO
        public void OpenWithoutCanClose()
        {
            Open();
            GlobalEvent.HideCanvas();

            foreach (var button in buttons)
            {
                button.gameObject.SetActive(false);
            }

            //TODO: set focus to first element in investigation screen
            OnSelectWindowHandler(NoteWindowScreensEnum.INVESTIGATION_SCREEN);
        }
#endif
        
        public void Hide()
        {
            OnClose?.Invoke();
        }

        public void SetLeftNavigationToButtons(Selectable selectable)
        {
            foreach (var screenButton in buttons)
            {
                ButtonExt btn = screenButton.GetComponent<ButtonExt>();
                if (btn)
                {
                    Navigation nav = btn.navigation;
                    nav.selectOnLeft = selectable;
                    btn.navigation = nav;
                }
            }
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