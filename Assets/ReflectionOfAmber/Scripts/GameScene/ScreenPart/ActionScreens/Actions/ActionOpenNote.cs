namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionOpenNote : ActionBase
    {
        public override void Action()
        {
            GlobalEvent.CallType(CallKeyType.NOTE_BOOKE_WITHOUT_EXIT);
        }

        public override ActionType ActionType => ActionType.OPEN_NOTE_ACTION;
    }
}