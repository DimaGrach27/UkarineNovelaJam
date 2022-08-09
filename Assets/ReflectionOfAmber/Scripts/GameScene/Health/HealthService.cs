using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.Health
{
    public class HealthService
    {
        private readonly HealthUiView _healthUiView;
        public HealthService(Transform transform)
        {
            _healthUiView = transform.GetComponentInChildren<HealthUiView>();

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
                    SceneService.LoadEndGame();
                }
                
                int health = Mathf.Clamp(value, 0, GlobalConstant.MAX_HEALTH);
                float amount = (float)health / GlobalConstant.MAX_HEALTH;
                _healthUiView.Health = amount;

                SaveService.HealthCount = health;
            }
        }
    }
}