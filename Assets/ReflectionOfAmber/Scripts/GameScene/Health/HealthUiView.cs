using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GameScene.Health
{
    public class HealthUiView : MonoBehaviour
    {
        [SerializeField] private Image fillBar;
        [SerializeField] private Animator animator;
        
        private static readonly int OnTakeDamage = Animator.StringToHash("OnTakeDamage");
        private static readonly int OnHeal = Animator.StringToHash("OnHeal");

        public float Health
        {
            set
            {
                fillBar.fillAmount = value;
            }
        }

        public void TakeDamage()
        {
            animator.SetTrigger(OnTakeDamage);
        }
        
        public void Heal()
        {
            animator.SetTrigger(OnHeal);
        }
    }
}