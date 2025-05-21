using ReflectionOfAmber.Scripts.Input;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameDebug_Test
{
    public class TestManager : MonoBehaviour
    {
        private InputService m_InputService;

        private void Awake()
        {
            m_InputService = new InputService();
        }

        private void Update()
        {
            m_InputService?.Tick();
        }
    }
}