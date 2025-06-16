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
            m_screenPartsService = screenPartsService;
            m_chapterNotesView = chapterNotesView;
            m_inputService = inputService;

            m_screenPartsService.OnOpenPart += OnChangePartHandler;
            GlobalEvent.OnCallType += OnOpenNotesHandler;
            m_inputService.AddListener(this);
        }

        private readonly ChapterNotesView m_chapterNotesView;
        private readonly InputService m_inputService;
        private readonly ScreenPartsService m_screenPartsService;
        
        private bool m_isChapterNotesOpen = false;

        private void OnOpenNotesHandler(CallKeyType callKeyType)
        {
            if(callKeyType != CallKeyType.OPEN_CHAPTERS)
            {
                return;
            }

            ShowChapterNotes();
        }

        private void ShowChapterNotes()
        {
            if (m_isChapterNotesOpen)
            {
                return;
            }
            
            ChapterNotesFile chapterNotesFile = SaveService.ChapterNotesFile;
            m_chapterNotesView.Open(chapterNotesFile.chapters);
            m_chapterNotesView.OnCloseButtonClick += CloseNotesHandler;
            m_inputService.ForceRedirectInput(this);
            
            m_isChapterNotesOpen = true;
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

        public void OnInputAction(InputActionEnum inputActionEnum)
        {
            if (inputActionEnum == InputActionEnum.CANCEL)
            {
                CloseNotesHandler();
            }

            if (inputActionEnum == InputActionEnum.LOG_SCREEN)
            {
                ShowChapterNotes();
            }
        }

        private void CloseNotesHandler()
        {
            if (!m_isChapterNotesOpen)
            {
                return;
            }
            
            m_chapterNotesView.OnCloseButtonClick -= CloseNotesHandler;
            m_chapterNotesView.Close();
            m_inputService.RemoveForceRedirected(this);

			m_isChapterNotesOpen = false;
        }

        public void Dispose()
        {
            m_inputService.RemoveListener(this);
            m_inputService.RemoveForceRedirected(this);
            GlobalEvent.OnCallType -= OnOpenNotesHandler;
            m_chapterNotesView.OnCloseButtonClick -= CloseNotesHandler;
            m_screenPartsService.OnOpenPart -= OnChangePartHandler;
        }
        
        public bool ShouldReceiveInput { get; set; } = true;
    }
}