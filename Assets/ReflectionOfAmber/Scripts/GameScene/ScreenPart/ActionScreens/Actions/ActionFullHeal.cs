using ReflectionOfAmber.Scripts.GameScene.Health;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionFullHeal : IActionScreen
    {
        private readonly HealthService _healthService;
        
        public ActionFullHeal(HealthService healthService)
        {
            _healthService = healthService;
        }
        
        public void Action()
        {
            _healthService.Health = GlobalConstant.MAX_HEALTH;
        }

        public ActionType ActionType => ActionType.HEAL_FULL_HEALTH;
    }
}