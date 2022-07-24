using GameScene.Health;

namespace GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionDamageOneHealth : IActionScreen
    {
        private readonly HealthService _healthService;
        
        public ActionDamageOneHealth(HealthService healthService)
        {
            _healthService = healthService;
        }
        
        public void Action()
        {
            _healthService.Health--;
        }

        public ActionType ActionType => ActionType.DAMAGE_ONE_HEALTH;
    }
}