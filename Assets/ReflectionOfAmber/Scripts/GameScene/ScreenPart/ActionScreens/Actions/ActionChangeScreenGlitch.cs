using ReflectionOfAmber.Scripts.GameScene.BgScreen;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionChangeScreenGlitch : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.BgService.Show(BgEnum.GLITCH_SCREEN, null);
        }

        public override ActionType ActionType => ActionType.CHANGE_GLITCH_SCREEN;
    }
    
    public class ActionChangeScreenDark : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.BgService.Show(BgEnum.DARKNESS, null);
        }

        public override ActionType ActionType => ActionType.CHANGE_DARK_SCREEN;
    }
    
    public class ActionChangeScreenForest : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.BgService.Show(BgEnum.FOREST_NORMAL, null);
        }

        public override ActionType ActionType => ActionType.CHANGE_FOREST_SCREEN;
    }
}