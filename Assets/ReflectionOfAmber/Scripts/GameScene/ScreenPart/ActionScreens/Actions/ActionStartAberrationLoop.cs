using ReflectionOfAmber.Scripts.GameScene.GlobalVolume;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionStartAberrationLoop : IActionScreen
    {
        public void Action()
        {
            GlobalVolumeService.Inst.PlayAberrationLoop();
        }

        public ActionType ActionType => ActionType.START_ABERRATION_LOOP;
    }
    
    public class ActionStopAberrationLoop : IActionScreen
    {
        public void Action()
        {
            GlobalVolumeService.Inst.StopAberrationLoop();
        }

        public ActionType ActionType => ActionType.STOP_ABERRATION_LOOP;
    }
}