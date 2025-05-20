namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionInstaShowNextPart : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.ScreenPartsServiceFacade.PlatNextPart();
        }

        public override ActionType ActionType => ActionType.INSTA_SHOW_NEXT_PART;
    }
}