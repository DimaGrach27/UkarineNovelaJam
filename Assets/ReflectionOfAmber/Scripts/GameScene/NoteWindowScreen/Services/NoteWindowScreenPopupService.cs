using System;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Misc;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Services
{
    public class NoteWindowScreenPopupService
    {
        public event Action<NoteWindowScreensEnum> OnSelectWindowClick;
        public event Action OnOpenNote;
        
        private readonly NoteWindowScreenPopup _noteWindowScreenPopup;
        
        [Inject]
        public NoteWindowScreenPopupService(NoteWindowScreenPopup noteWindowScreenPopup)
        {
            _noteWindowScreenPopup = noteWindowScreenPopup;
            _noteWindowScreenPopup.OnSelectWindowClick += OnSelectWindowHandler;
            GlobalEvent.OnCallType += OnOpenHandler;
            GlobalEvent.OnCallType += OnOpenWithoutCanCloseHandler;
        }

        private void OnOpenHandler(CallKeyType callKeyType)
        {
            if(callKeyType != CallKeyType.NOTE_BOOKE) return;
            
            _noteWindowScreenPopup.Open();
            OnOpenNote?.Invoke();
        }

        private void OnOpenWithoutCanCloseHandler(CallKeyType callKeyType)
        {
            if(callKeyType != CallKeyType.NOTE_BOOKE_WITHOUT_EXIT) return;
            _noteWindowScreenPopup.OpenWithoutCanClose();
        }
        
        private void OnSelectWindowHandler(NoteWindowScreensEnum noteWindowScreensEnum)
        {
            OnSelectWindowClick?.Invoke(noteWindowScreensEnum);
        }
    }
}