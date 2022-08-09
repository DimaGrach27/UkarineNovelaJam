namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionStartAberrationLoop : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.GlobalVolumeService.PlayAberrationLoop();
        }

        public override ActionType ActionType => ActionType.START_ABERRATION_LOOP;
    }
    
    public class ActionStopAberrationLoop : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.GlobalVolumeService.StopAberrationLoop();
        }

        public override ActionType ActionType => ActionType.STOP_ABERRATION_LOOP;
    }
}