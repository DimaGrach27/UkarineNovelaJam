using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GameDebug_Test
{
    public class TestScrollInput : MonoBehaviour
    {
        [SerializeField] private float m_mouseScrollValue;
        
        [SerializeField] private ScrollRect m_scrollRect;
        
        void Update()
        {
            m_scrollRect.verticalScrollbar.value += m_mouseScrollValue;
            m_mouseScrollValue = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
            print($"Mouse scroll: {m_mouseScrollValue}");
        }
    }
}