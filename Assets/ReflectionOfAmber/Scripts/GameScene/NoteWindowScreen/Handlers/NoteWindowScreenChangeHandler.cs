using System;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Misc;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Services;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Handlers
{
    public class NoteWindowScreenChangeHandler : IDisposable
    {
        private readonly Dictionary<NoteWindowScreensEnum, INoteWindowScreen> _noteWindowScreensMap;

        private INoteWindowScreen m_noteWindowScreen;
        private readonly NoteWindowScreenPopupService m_NoteWindowScreenPopupService;
        private readonly NoteWindowScreenPopup m_noteWindowScreenPopup;
        
        [Inject]
        public NoteWindowScreenChangeHandler(
            List<INoteWindowScreen> noteWindowScreens,
            NoteWindowScreenPopupService noteWindowScreenPopupService,
            NoteWindowScreenPopup noteWindowScreenPopup
            )
        {
            _noteWindowScreensMap = new();
            m_NoteWindowScreenPopupService = noteWindowScreenPopupService;
            m_noteWindowScreenPopup = noteWindowScreenPopup;
            
            foreach (var iNoteWindowScreen in noteWindowScreens)
            {
                _noteWindowScreensMap.Add(iNoteWindowScreen.NoteWindowScreensEnum, iNoteWindowScreen);
            }

            // m_NoteWindowScreenPopupService.OnSelectWindowClick += OnSelectWindowHandler;
            m_NoteWindowScreenPopupService.OnOpenNote += OnOpenNoteHandler;
            m_noteWindowScreenPopup.OnSelectWindowClick += OnSelectWindowHandler;
        }

        private void OnOpenNoteHandler()
        {
            CloseAllWindow();
            OnSelectWindowHandler(NoteWindowScreensEnum.MAIN_SCREEN);
        }
        
        private void CloseAllWindow()
        {
            foreach (var iNoteWindow in _noteWindowScreensMap.Values)
            {
                iNoteWindow.Close();
            }
        }

        private void OnSelectWindowHandler(NoteWindowScreensEnum noteWindowScreensEnum)
        {
            if(m_noteWindowScreen != null) m_noteWindowScreen.Close();
            if(_noteWindowScreensMap.ContainsKey(noteWindowScreensEnum))
            {
                m_noteWindowScreen = _noteWindowScreensMap[noteWindowScreensEnum];
                m_noteWindowScreen.Open();
                
                // m_noteWindowScreenPopup.SetLeftNavigationToButtons(m_noteWindowScreen.GetFirstSelectable());
            }
        }

        public void Dispose()
        {
            // m_NoteWindowScreenPopupService.OnSelectWindowClick -= OnSelectWindowHandler;
            m_NoteWindowScreenPopupService.OnOpenNote -= OnOpenNoteHandler;
            m_noteWindowScreenPopup.OnSelectWindowClick -= OnSelectWindowHandler;
        }
    }
}