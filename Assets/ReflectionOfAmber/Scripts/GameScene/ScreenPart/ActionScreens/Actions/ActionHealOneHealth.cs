namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionHealOneHealth : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.HealthService.Health++;
        }

        public override ActionType ActionType => ActionType.HEAL_ONE_HEALTH;
    }
}