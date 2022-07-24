using UnityEngine;

namespace GameScene.Health
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
                int health = Mathf.Clamp(value, 0, GlobalConstant.MAX_HEALTH);
                _healthUiView.Health = health;
                SaveService.HealthCount = health;
            }
        }
    }
}