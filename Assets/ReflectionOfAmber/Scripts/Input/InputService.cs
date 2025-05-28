using System;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using Zenject;

namespace ReflectionOfAmber.Scripts.Input
{
    public class InputService : IInit, ITickable
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
            if (m_forceRedirected.Count == 0)
            {
                return;
            }
            
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
                if(inputListener.ShouldReceiveInput)
                {
                    inputListener.OnInputAction(inputAction);
                }
            }
        }

        public void Tick()
        {
            if (Mouse.current.delta.value.magnitude > 0.1f)
            {
                Cursor.visible = true;
            }
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                SetAction(InputAction.PAUSE);
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                SetAction(InputAction.SPACE);
            }
            
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                bool hasSelectedObject = EventSystem.current.currentSelectedGameObject;
                bool isClickOnUI = EventSystem.current.IsPointerOverGameObject();
                
                // bool hasDragObject = EventSystem.current.currentInputModule;
             
                if(!hasSelectedObject && !isClickOnUI)
                {
                    SetAction(InputAction.LEFT_MOUSE);
                }

                // 
                // string pressedObject = hasSelectedObject ? EventSystem.current.currentSelectedGameObject.name : String.Empty;
                // Debug.Log($"Mouse click on UI [isClickOnUI = {isClickOnUI}] [hasSelectedObject = {hasSelectedObject}]");
            }
        }

        public event Action OnReady;
        public void Init()
        {
            SetupInputEvents();
            
            OnReady?.Invoke();
        }

        private void SetupInputEvents()
        {
            UnityEngine.InputSystem.InputAction inputAction = InputSystem.actions.FindAction("Submit");
            if (inputAction != null)
            {
                inputAction.performed += InputActionPerformed;
            }
        }

        private void InputActionPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            Debug.Log($"InputActionPerformed: {obj.action.name}");
            Cursor.visible = false;
        }
    }

    public enum InputAction
    {
        NONE,
        PAUSE,
        SPACE,
        LEFT_MOUSE
    }
}