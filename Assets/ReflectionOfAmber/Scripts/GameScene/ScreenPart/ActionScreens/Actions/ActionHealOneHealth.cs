using ReflectionOfAmber.Scripts.GameScene.Health;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionHealOneHealth : IActionScreen
    {
        private readonly HealthService _healthService;
        
        public ActionHealOneHealth(HealthService healthService)
        {
            _healthService = healthService;
        }
        
        public void Action()
        {
            _healthService.Health++;
        }

        public ActionType ActionType => ActionType.HEAL_ONE_HEALTH;
    }
}