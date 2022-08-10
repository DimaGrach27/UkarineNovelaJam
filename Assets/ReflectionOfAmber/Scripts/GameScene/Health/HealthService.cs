using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.Health
{
    public class HealthService
    {
        private readonly HealthUiView _healthUiView;
        private readonly SceneService _sceneService;
        
        [Inject]
        public HealthService(GamePlayCanvas gamePlayCanvas, SceneService sceneService)
        {
            _healthUiView = gamePlayCanvas.GetComponentInChildren<HealthUiView>();
            _sceneService = sceneService;
            
            Health = Health;
        }
        
        public int Health
        {
            get => SaveService.HealthCount;

            set
            {
                if (value < Health)
                {
                    _healthUiView.TakeDamage();
                }
                
                if (value > Health)
                {
                    _healthUiView.Heal();
                }
                
                if (value <= 0)
                {
                    _sceneService.LoadEndGame();
                }
                
                int health = Mathf.Clamp(value, 0, GlobalConstant.MAX_HEALTH);
                float amount = (float)health / GlobalConstant.MAX_HEALTH;
                _healthUiView.Health = amount;

                SaveService.HealthCount = health;
            }
        }
    }
}