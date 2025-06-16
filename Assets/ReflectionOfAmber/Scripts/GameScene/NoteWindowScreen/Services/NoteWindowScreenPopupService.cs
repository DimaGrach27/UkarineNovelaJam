using System;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views;
using ReflectionOfAmber.Scripts.Input;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Services
{
    public class NoteWindowScreenPopupService : IInputListener, IDisposable
    {
        // public event Action<NoteWindowScreensEnum> OnSelectWindowClick;
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
            // _noteWindowScreenPopup.OnSelectWindowClick += OnSelectWindowHandler;
            GlobalEvent.OnCallType += OnOpenHandler;
#if !GAME_DEMO

            GlobalEvent.OnCallType += OnOpenWithoutCanCloseHandler;
#endif

            m_inputService.AddListener(this);
        }

        private void OnOpenHandler(CallKeyType callKeyType)
        {
            if(callKeyType != CallKeyType.NOTE_BOOK)
            {
                return;
            }
            
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
            // m_inputService.ForceBlockInput(false);
            m_inputService.RemoveForceRedirected(this);

            m_isNoteWindowOpened = false;
        }

#if !GAME_DEMO
        private void OnOpenWithoutCanCloseHandler(CallKeyType callKeyType)
        {
            if(callKeyType != CallKeyType.NOTE_BOOKE_WITHOUT_EXIT)
            {
                return;
            }

            _noteWindowScreenPopup.OpenWithoutCanClose();
            m_inputService.ForceRedirectInput(this);
            // m_inputService.ForceBlockInput(true);
        }
#endif
        
        // private void OnSelectWindowHandler(NoteWindowScreensEnum noteWindowScreensEnum)
        // {
        //     OnSelectWindowClick?.Invoke(noteWindowScreensEnum);
        // }

        public void OnInputAction(InputAction inputAction)
        {
            // if (m_isNoteWindowOpened)
            // {
            //     if (inputAction == InputAction.CANCEL)
            //     {
            //         CloseHandler();
            //     }
            // }
            // else
            // {
            //     if (inputAction == InputAction.NOTE_SCREEN)
            //     {
            //         OnOpenHandler(CallKeyType.NOTE_BOOK);
            //     }
            // }
            // if (inputAction == InputAction.CANCEL)
            // {
            //     if (m_isNoteWindowOpened)
            //     {
            //         CloseHandler();
            //     }
            // }
            //
            if (inputAction == InputAction.NOTE_SCREEN)
            {
                if(!m_isNoteWindowOpened)
                {
                    OnOpenHandler(CallKeyType.NOTE_BOOK);
                }
                else
                {
                    CloseHandler();
                }
            }

            if (inputAction == InputAction.TAB_NAVIGATION_LEFT)
            {
                _noteWindowScreenPopup.MoveUpTabNavigation();
            }
            else if(inputAction == InputAction.TAB_NAVIGATION_RIGHT)
            {
                _noteWindowScreenPopup.MoveDownTabNavigation();
            }
        }

        public bool ShouldReceiveInput { get; set; } = true;

        public void Dispose()
        {
            m_inputService.RemoveForceRedirected(this);
            // m_inputService.ForceBlockInput(false);
            m_inputService.RemoveListener(this);
            
            // _noteWindowScreenPopup.OnSelectWindowClick -= OnSelectWindowHandler;
            GlobalEvent.OnCallType -= OnOpenHandler;
#if !GAME_DEMO
            GlobalEvent.OnCallType -= OnOpenWithoutCanCloseHandler;
#endif
        }
    }
}