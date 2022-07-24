using UnityEngine;
using UnityEngine.UI;

namespace GameScene.Health
{
    public class HealthUiView : MonoBehaviour
    {
        [SerializeField] private Image[] images;

        public int Health
        {
            set
            {
                foreach (var image in images)
                {
                    image.enabled = false;
                }

                for (int i = 0; i < value; i++)
                {
                    images[i].enabled = true;
                }
            }
        }
    }
}