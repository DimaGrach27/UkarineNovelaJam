using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.UI
{
    public class MouseInteractBlocker : MonoBehaviour
    {
        private Image m_blockerImage;

        private void Awake()
        {
            m_blockerImage = GetComponent<Image>();
        }

        public void SetBlock(bool isBlock)
        {
            m_blockerImage.enabled = isBlock;
        }
    }
}