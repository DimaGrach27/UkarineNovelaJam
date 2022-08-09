using ReflectionOfAmber.Scripts.GameScene.BgScreen;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionChangeScreenGlitch : IActionScreen
    {
        private readonly BgService _bgService;
        
        public ActionChangeScreenGlitch(BgService bgService)
        {
            _bgService = bgService;
        }
        
        public void Action()
        {
            _bgService.Show(BgEnum.GLITCH_SCREEN, null);
        }

        public ActionType ActionType => ActionType.CHANGE_GLITCH_SCREEN;
    }
    
    public class ActionChangeScreenDark : IActionScreen
    {
        private readonly BgService _bgService;
        
        public ActionChangeScreenDark(BgService bgService)
        {
            _bgService = bgService;
        }
        
        public void Action()
        {
            _bgService.Show(BgEnum.DARKNESS, null);
        }

        public ActionType ActionType => ActionType.CHANGE_DARK_SCREEN;
    }
    
    public class ActionChangeScreenForest : IActionScreen
    {
        private readonly BgService _bgService;
        
        public ActionChangeScreenForest(BgService bgService)
        {
            _bgService = bgService;
        }
        
        public void Action()
        {
            _bgService.Show(BgEnum.FOREST_NORMAL, null);
        }

        public ActionType ActionType => ActionType.CHANGE_FOREST_SCREEN;
    }
}