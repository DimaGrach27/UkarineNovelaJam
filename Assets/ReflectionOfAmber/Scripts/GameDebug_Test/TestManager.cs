using ReflectionOfAmber.Scripts.Input;
using ReflectionOfAmber.Scripts.UI;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameDebug_Test
{
    public class TestManager : MonoBehaviour
    {
        [SerializeField] private MouseInteractBlocker mouseInteractBlocker;
        private InputService m_InputService;

        private void Awake()
        {
            m_InputService = new InputService(mouseInteractBlocker);
            
            m_InputService.Init();
        }
        
        private void Update()
        {
            m_InputService?.Tick();
        }
    }
}