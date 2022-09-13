using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GlobalProject;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.ChapterNotes
{
    public class ChapterNotesService
    {
        [Inject]
        public ChapterNotesService(ScreenPartsService screenPartsService, 
            ChapterNotesView chapterNotesView)
        {
            _chapterNotesView = chapterNotesView;

            screenPartsService.OnOpenPart += OnChangePartHandler;
            GlobalEvent.OnCallType += OnOpenNotesHandler;
        }

        private readonly ChapterNotesView _chapterNotesView;

        private void OnOpenNotesHandler(CallKeyType callKeyType)
        {
            if(callKeyType != CallKeyType.OPEN_CHAPTERS) return;
            
            ChapterNotesFile chapterNotesFile = SaveService.ChapterNotesFile;
            _chapterNotesView.Open(chapterNotesFile.chapters);
        }
        
        private void OnChangePartHandler(int part)
        {
            string currentScene = SaveService.GetScene;
            ScreenSceneScriptableObject sceneSo = GameModel.GetScene(currentScene);
            ScreenPart.ScreenPart screenPart = sceneSo.ScreenParts[part];
            
            SetDialogPart(screenPart.CharacterName, screenPart.TextShow);
        }
        
        private void SetDialogPart(string name, string text)
        {
            ChapterNotesFile chapterNotesFile = SaveService.ChapterNotesFile;
            NoteChapterPart noteChapterPart = new NoteChapterPart
            {
                name = name,
                text = text
            };
            
            chapterNotesFile.chapters.Add(noteChapterPart);
            SaveService.SaveChapterNotesJson();
        }
    }
}