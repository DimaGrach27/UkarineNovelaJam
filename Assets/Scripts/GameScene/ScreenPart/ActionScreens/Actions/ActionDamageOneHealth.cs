using GameScene.Health;

namespace GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionDamageOneHealth : IActionScreen
    {
        private readonly HealthService _healthService;
        private readonly ActionScreenService _actionScreenService;
        
        public ActionDamageOneHealth(HealthService healthService, 
            ActionScreenService actionScreenService)
        {
            _healthService = healthService;
            _actionScreenService = actionScreenService;
        }
        
        public void Action()
        {
            _healthService.Health--;
            _actionScreenService.Action(ActionType.CAMERA_SHAKER);
        }

        public ActionType ActionType => ActionType.DAMAGE_ONE_HEALTH;
    }
}