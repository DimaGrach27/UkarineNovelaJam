using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GameDebug_Test
{
    public class TestScrollInput : MonoBehaviour
    {
        [SerializeField] private ScrollRect m_scrollRect;
        
        void Update()
        {
            m_scrollRect.verticalScrollbar.ScrollByMouseWheel();
        }
    }
}