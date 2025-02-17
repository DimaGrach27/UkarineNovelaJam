using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Misc;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Services
{
    public class NoteWindowContinueScreen : INoteWindowScreen
    {
        [Inject]
        public NoteWindowContinueScreen(NoteWindowScreenPopup noteWindowScreenPopup)
        {
            _noteWindowScreenPopup = noteWindowScreenPopup;
        }

        private readonly NoteWindowScreenPopup _noteWindowScreenPopup;
        
        public void Open()
        {
            _noteWindowScreenPopup.Hide();
        }

        public void Close()
        {
            
        }

        public NoteWindowScreensEnum NoteWindowScreensEnum => NoteWindowScreensEnum.RESUME;
    }
}