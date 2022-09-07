using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Misc;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Services;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Handlers
{
    public class NoteWindowScreenChangeHandler
    {
        private readonly Dictionary<NoteWindowScreensEnum, INoteWindowScreen> _noteWindowScreensMap;

        private INoteWindowScreen _noteWindowScreen;
        
        [Inject]
        public NoteWindowScreenChangeHandler(List<INoteWindowScreen> noteWindowScreens,
            NoteWindowScreenPopupService noteWindowScreenPopupService)
        {
            _noteWindowScreensMap = new();

            foreach (var iNoteWindowScreen in noteWindowScreens)
            {
                _noteWindowScreensMap.Add(iNoteWindowScreen.NoteWindowScreensEnum, iNoteWindowScreen);
            }

            noteWindowScreenPopupService.OnSelectWindowClick += OnSelectWindowHandler;
            noteWindowScreenPopupService.OnOpenNote += OnOpenNoteHandler;
        }

        private void OnOpenNoteHandler()
        {
            OnSelectWindowHandler(NoteWindowScreensEnum.MAIN_SCREEN);
        }

        private void OnSelectWindowHandler(NoteWindowScreensEnum noteWindowScreensEnum)
        {
            if(_noteWindowScreen != null) _noteWindowScreen.Close();
            if(_noteWindowScreensMap.ContainsKey(noteWindowScreensEnum))
            {
                _noteWindowScreen = _noteWindowScreensMap[noteWindowScreensEnum];
                _noteWindowScreen.Open();
            }
        }
    }
}