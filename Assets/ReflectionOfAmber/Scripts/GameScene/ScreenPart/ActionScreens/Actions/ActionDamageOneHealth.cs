namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionDamageOneHealth : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.HealthService.Health--;
            ActionScreenService.Action(ActionType.CAMERA_SHAKER);
        }

        public override ActionType ActionType => ActionType.DAMAGE_ONE_HEALTH;
    }
}