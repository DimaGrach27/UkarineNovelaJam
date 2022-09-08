using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Misc;
using ReflectionOfAmber.Scripts.GlobalProject;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Services
{
    public class NoteWindowExitScreen : INoteWindowScreen
    {
        [Inject]
        public NoteWindowExitScreen(ConfirmScreen confirmScreen, 
            SceneService sceneService)
        {
            _confirmScreen = confirmScreen;
            _sceneService = sceneService;
        }

        private readonly ConfirmScreen _confirmScreen;
        private readonly SceneService _sceneService;
        
        public void Open()
        {
            _confirmScreen.Check(ConfirmHandler, "Ви точно бажаєте вийти?");
        }

        public void Close()
        {
            
        }

        private void ConfirmHandler(bool isConfirm)
        {
            if(!isConfirm) return;
            GameModel.IsGamePlaying = false;
            _sceneService.LoadMainMenuScene();
            
        }

        public NoteWindowScreensEnum NoteWindowScreensEnum => NoteWindowScreensEnum.EXIT;
    }
}