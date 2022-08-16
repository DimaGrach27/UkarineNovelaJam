using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GlobalProject;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.ChapterNotes
{
    public class ChapterNotesService
    {
        [Inject]
        public ChapterNotesService(
            ScreenPartsService screenPartsService,
            ChapterNotesSaveService chapterNotesSaveService
            )
        {
            _screenPartsService = screenPartsService;
            _chapterNotesSaveService = chapterNotesSaveService;

            screenPartsService.OnOpenPart += OnChangePartHandler;
            screenPartsService.OnOpenScene += OnChangeSceneHandler;
        }
        
        private readonly ScreenPartsService _screenPartsService;
        private readonly ChapterNotesSaveService _chapterNotesSaveService;

        private void OnChangePartHandler(int part)
        {
            string currentScene = SaveService.GetScene;
            ScreenSceneScriptableObject sceneSo = GameModel.GetScene(currentScene);
            ScreenPart.ScreenPart screenPart = sceneSo.ScreenParts[part];
            
            _chapterNotesSaveService.SetDialogPart(screenPart.CharacterName, screenPart.TextShow);
        }

        private void OnChangeSceneHandler(string key)
        {
            
        }
    }
}