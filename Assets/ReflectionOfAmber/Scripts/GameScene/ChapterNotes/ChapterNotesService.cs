using System;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.Input;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.ChapterNotes
{
    public class ChapterNotesService : IInputListener, IDisposable
    {
        [Inject]
        public ChapterNotesService(
            ScreenPartsService screenPartsService, 
            ChapterNotesView chapterNotesView,
            InputService inputService
            )
        {
            _chapterNotesView = chapterNotesView;
            m_inputService = inputService;

            screenPartsService.OnOpenPart += OnChangePartHandler;
            GlobalEvent.OnCallType += OnOpenNotesHandler;
        }

        private readonly ChapterNotesView _chapterNotesView;
        private readonly InputService m_inputService;

        private void OnOpenNotesHandler(CallKeyType callKeyType)
        {
            if(callKeyType != CallKeyType.OPEN_CHAPTERS) return;
            
            ChapterNotesFile chapterNotesFile = SaveService.ChapterNotesFile;
            _chapterNotesView.Open(chapterNotesFile.chapters);
            _chapterNotesView.OnCloseButtonClick += CloseNotesHandler;
            m_inputService.ForceRedirectInput(this);
        }
        
        private void OnChangePartHandler(int part)
        {
            string currentScene = SaveService.GetScene;
            ScreenSceneScriptableObject sceneSo = GameModel.GetScene(currentScene);
            ScreenPart.ScreenPart screenPart = sceneSo.ScreenParts[part];

            string textKey = $"{sceneSo.SceneKey}_part_{part + 1}";
            
            SetDialogPart(screenPart.CharacterNameType, textKey);
        }
        
        private void SetDialogPart(string name, string text)
        {
            ChapterNotesFile chapterNotesFile = SaveService.ChapterNotesFile;
            if(chapterNotesFile.chapters.Count != 0)
            {
                NoteChapterPart lastNoteChapterPart = chapterNotesFile.chapters[^1];
                if (lastNoteChapterPart.name == name && lastNoteChapterPart.text == text) return;
            }
            
            NoteChapterPart noteChapterPart = new NoteChapterPart
            {
                name = name,
                text = text
            };
            
            chapterNotesFile.chapters.Add(noteChapterPart);
            SaveService.SaveChapterNotesJson();
        }

        public void OnInputAction(InputAction inputAction)
        {
            if (inputAction == InputAction.PAUSE)
            {
                CloseNotesHandler();
                m_inputService.RemoveForceRedirected(this);
            }
        }

        private void CloseNotesHandler()
        {
            _chapterNotesView.OnCloseButtonClick -= CloseNotesHandler;
            _chapterNotesView.Close();
        }

        public void Dispose()
        {
            m_inputService.RemoveForceRedirected(this);
        }
    }
}