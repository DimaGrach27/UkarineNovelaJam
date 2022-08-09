namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionFullHeal : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.HealthService.Health = GlobalConstant.MAX_HEALTH;
        }

        public override ActionType ActionType => ActionType.HEAL_FULL_HEALTH;
    }
}