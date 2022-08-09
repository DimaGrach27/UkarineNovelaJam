using ReflectionOfAmber.Scripts.GameScene.BgScreen;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionChangeScreenGlitch : IActionScreen
    {
        private readonly ScreenPartsService _screenPartsService;
        
        public ActionChangeScreenGlitch(ScreenPartsService screenPartsService)
        {
            _screenPartsService = screenPartsService;
        }
        
        public void Action()
        {
            _screenPartsService.ChangeBack(BgEnum.GLITCH_SCREEN);
        }

        public ActionType ActionType => ActionType.CHANGE_GLITCH_SCREEN;
    }
    
    public class ActionChangeScreenDark : IActionScreen
    {
        private readonly ScreenPartsService _screenPartsService;
        
        public ActionChangeScreenDark(ScreenPartsService screenPartsService)
        {
            _screenPartsService = screenPartsService;
        }
        
        public void Action()
        {
            _screenPartsService.ChangeBack(BgEnum.DARKNESS);
        }

        public ActionType ActionType => ActionType.CHANGE_DARK_SCREEN;
    }
    
    public class ActionChangeScreenForest : IActionScreen
    {
        private readonly ScreenPartsService _screenPartsService;
        
        public ActionChangeScreenForest(ScreenPartsService screenPartsService)
        {
            _screenPartsService = screenPartsService;
        }
        
        public void Action()
        {
            _screenPartsService.ChangeBack(BgEnum.FOREST_NORMAL);
        }

        public ActionType ActionType => ActionType.CHANGE_FOREST_SCREEN;
    }
}