using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.Input
{
    public class InputService : ITickable
    {
        private List<IInputListener> m_listeners = new();

        private Stack<IInputListener> m_forceRedirected = new();

        private bool m_isInputBlocked;
        
        public void AddListener(IInputListener inputListener)
        {
            m_listeners.Add(inputListener);
        }

        public void RemoveListener(IInputListener inputListener)
        {
            m_listeners.Remove(inputListener);
        }

        public void ForceBlockInput(bool value)
        {
            m_isInputBlocked = value;
        }

        public void ForceRedirectInput(IInputListener inputListener)
        {
            m_forceRedirected.Push(inputListener);
        }

        public void RemoveForceRedirected(IInputListener inputListener)
        {
            if (m_forceRedirected.Peek() != inputListener)
            {
                Debug.LogError($"Wrong redirected listener: {inputListener.GetType()}, current is {m_forceRedirected.GetType()}");
                return;
            }

            m_forceRedirected.Pop();
        }

        private void SetAction(InputAction inputAction)
        {
            if (m_isInputBlocked)
            {
                return;
            }
            
            if (m_forceRedirected.Count > 0)
            {
                m_forceRedirected.Peek().OnInputAction(inputAction);
                return;
            }

            foreach (var inputListener in m_listeners)
            {
                inputListener.OnInputAction(inputAction);
            }
        }

        public void Tick()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                SetAction(InputAction.PAUSE);
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                SetAction(InputAction.SPACE);
            }
        }
    }

    public enum InputAction
    {
        NONE,
        PAUSE,
        SPACE
    }
}