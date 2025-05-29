using System;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Misc;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views;
using ReflectionOfAmber.Scripts.Input;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Services
{
    public class NoteWindowScreenPopupService : IInputListener, IDisposable
    {
        public event Action<NoteWindowScreensEnum> OnSelectWindowClick;
        public event Action OnOpenNote;
        
        private readonly NoteWindowScreenPopup _noteWindowScreenPopup;
        private readonly InputService m_inputService;

        private bool m_isNoteWindowOpened;

        [Inject]
        public NoteWindowScreenPopupService(
            NoteWindowScreenPopup noteWindowScreenPopup,
            InputService inputService
            )
        {
            _noteWindowScreenPopup = noteWindowScreenPopup;
            m_inputService = inputService;
            _noteWindowScreenPopup.OnSelectWindowClick += OnSelectWindowHandler;
            GlobalEvent.OnCallType += OnOpenHandler;
#if !GAME_DEMO

            GlobalEvent.OnCallType += OnOpenWithoutCanCloseHandler;
#endif

            m_inputService.AddListener(this);
        }

        private void OnOpenHandler(CallKeyType callKeyType)
        {
            if(callKeyType != CallKeyType.NOTE_BOOKE) return;
            
            _noteWindowScreenPopup.OnClose += CloseHandler;
            _noteWindowScreenPopup.OpenWithClose();
            OnOpenNote?.Invoke();

            m_inputService.ForceRedirectInput(this);
            m_isNoteWindowOpened = true;
        }

        private void CloseHandler()
        {
            _noteWindowScreenPopup.OnClose -= CloseHandler;

            _noteWindowScreenPopup.Close();
            m_inputService.ForceBlockInput(false);
            m_inputService.RemoveForceRedirected(this);

            m_isNoteWindowOpened = false;
        }

#if !GAME_DEMO
        private void OnOpenWithoutCanCloseHandler(CallKeyType callKeyType)
        {
            if(callKeyType != CallKeyType.NOTE_BOOKE_WITHOUT_EXIT) return;
            _noteWindowScreenPopup.OpenWithoutCanClose();
            m_inputService.ForceBlockInput(true);
        }
#endif
        
        private void OnSelectWindowHandler(NoteWindowScreensEnum noteWindowScreensEnum)
        {
            OnSelectWindowClick?.Invoke(noteWindowScreensEnum);
        }

        public void OnInputAction(InputAction inputAction)
        {
            if (inputAction == InputAction.CANCEL)
            {
                if (m_isNoteWindowOpened)
                {
                    CloseHandler();
                }
                else
                {
                    OnOpenHandler(CallKeyType.NOTE_BOOKE);
                }
            }
        }

        public bool ShouldReceiveInput { get; set; } = true;

        public void Dispose()
        {
            m_inputService.RemoveForceRedirected(this);
            m_inputService.ForceBlockInput(false);
            m_inputService.RemoveListener(this);
            
            _noteWindowScreenPopup.OnSelectWindowClick -= OnSelectWindowHandler;
            GlobalEvent.OnCallType -= OnOpenHandler;
#if !GAME_DEMO
            GlobalEvent.OnCallType -= OnOpenWithoutCanCloseHandler;
#endif
        }
    }
}