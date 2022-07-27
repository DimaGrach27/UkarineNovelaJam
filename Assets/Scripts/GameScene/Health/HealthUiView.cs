using UnityEngine;
using UnityEngine.UI;

namespace GameScene.Health
{
    public class HealthUiView : MonoBehaviour
    {
        [SerializeField] private Image fillBar;

        public float Health
        {
            set
            {
                fillBar.fillAmount = value;
            }
        }
        
    }
}