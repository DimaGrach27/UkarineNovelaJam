using System;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

namespace ReflectionOfAmber.Scripts.Input
{
    public class InputService : IInit, ITickable
    {
        private readonly MouseInteractBlocker m_mouseInteractBlocker;
        private List<IInputListener> m_listeners = new();

        private Stack<IInputListener> m_forceRedirected = new();

        private bool m_isInputBlocked;
        // private int m_lastPerformedFrame;
        // private Queue<InputAction> m_inputActions = new();
        
        private UnityEngine.InputSystem.InputAction TabNavigation;
        
        [Inject]
        public InputService(MouseInteractBlocker mouseInteractBlocker)
        {
            m_mouseInteractBlocker = mouseInteractBlocker;
        }
        
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

        private void SetAction(InputActionEnum inputActionEnum)
        {
            if (m_isInputBlocked)
            {
                return;
            }
            
            if (m_forceRedirected.Count > 0)
            {
                m_forceRedirected.Peek().OnInputAction(inputActionEnum);
                return;
            }

            foreach (var inputListener in m_listeners)
            {
                if(inputListener.ShouldReceiveInput)
                {
                    inputListener.OnInputAction(inputActionEnum);
                }
            }
        }

        public void Tick()
        {
            if (Mouse.current.delta.value.magnitude > 0.1f)
            {
                BlockAndHideMouse(false);
            }
            
            // if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            // {
            //     SetAction(InputAction.CANCEL);
            // }
            //
            // if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            // {
            //     SetAction(InputAction.SPACE);
            // }
            //
            // if (UnityEngine.Input.GetMouseButtonDown(0))
            // {
            //     bool hasSelectedObject = EventSystem.current.currentSelectedGameObject;
            //     bool isClickOnUI = EventSystem.current.IsPointerOverGameObject();
            //     
            //     // bool hasDragObject = EventSystem.current.currentInputModule;
            //  
            //     if(!hasSelectedObject && !isClickOnUI)
            //     {
            //         SetAction(InputAction.LEFT_MOUSE);
            //     }
            //
            //     // 
            //     // string pressedObject = hasSelectedObject ? EventSystem.current.currentSelectedGameObject.name : String.Empty;
            //     // Debug.Log($"Mouse click on UI [isClickOnUI = {isClickOnUI}] [hasSelectedObject = {hasSelectedObject}]");
            // }
            
            
        }

        public event Action OnReady;
        public void Init()
        {
            SetupInputEvents();
            
            OnReady?.Invoke();
        }

        private void SetupInputEvents()
        {
            InputActionMap uiActionMap = InputSystem.actions.FindActionMap("UI");
            
            InputSystem.actions.FindActionMap("Player")?.Disable();
            
            foreach (var inputAction in uiActionMap.actions)
            {
                if (inputAction != null)
                {
                    inputAction.performed += InputActionPerformed;
                    inputAction.started += InputActionStarted;
                    inputAction.canceled += InputActionCanceled;
                }
            }
            
            TabNavigation = InputSystem.actions.FindAction("TabNavigation");
        }

        private void InputActionCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            // Debug.Log($"InputActionCanceled: {obj.action.name}");
        }
        
        private void InputActionStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            // Debug.Log($"InputActionStarted: {obj.action.name}");
        }

        private void InputActionPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            Debug.Log($"Current device: {obj.control.device.path}");
            
            // DeviceType currentDeviceType;
            // Enum.TryParse<DeviceType>(obj.control.device.name, true, out currentDeviceType);
            // Debug.Log($"Current device: {currentDeviceType}");
            
            // Debug.Log($"InputActionPerformed: {obj.action.name}");

            switch (obj.action.name)
            {
                case "Submit":
                {
                    // m_inputActions.Enqueue(InputAction.SUBMIT);
                    SetAction(InputActionEnum.SUBMIT); 
                    BlockAndHideMouse(true);
                    break;
                }
                
                case "Cancel":
                {
                    // m_inputActions.Enqueue(InputAction.CANCEL);
                    SetAction(InputActionEnum.CANCEL);
                    BlockAndHideMouse(true);
                    break;
                }
                
                case "Navigate":
                {
                    BlockAndHideMouse(true);
                    break;
                }
                
                case "LogScreen":
                {
                    // m_inputActions.Enqueue(InputAction.LOG_SCREEN);
                    SetAction(InputActionEnum.LOG_SCREEN);
                    BlockAndHideMouse(true);
                    break;
                }
                
                case "NoteScreen":
                {
                    // m_inputActions.Enqueue(InputAction.NOTE_SCREEN);
                    SetAction(InputActionEnum.NOTE_SCREEN);
                    BlockAndHideMouse(true);
                    break;
                }
                
                // case "TabNavigation":
                // {
                //     float val = TabNavigation.ReadValue<float>();
                //     SetAction(val > 0 ? InputActionEnum.TAB_NAVIGATION_RIGHT : InputActionEnum.TAB_NAVIGATION_LEFT);
                //     BlockAndHideMouse(true);
                //     break;
                // }

                case "TabNavigation_Right":
                {

                    SetAction(InputActionEnum.TAB_NAVIGATION_RIGHT);
                    BlockAndHideMouse(true);
                    break;
                }

                case "TabNavigation_Left":
                {
                    SetAction(InputActionEnum.TAB_NAVIGATION_LEFT);
                    BlockAndHideMouse(true);
                    break;
                }
                
                case "Click":
                {
                    // bool hasSelectedObject = EventSystem.current.currentSelectedGameObject;
                    bool isClickOnUI = EventSystemUtility.IsPointerOverGUIAction();
                    // bool isClickOnUI = EventSystem.current.IsPointerOverGameObject();
                    
                    if(!isClickOnUI)
                    // if(!hasSelectedObject && !isClickOnUI)
                    {
                        // Debug.Log("Actually was clicked!");
                        // m_inputActions.Enqueue(InputAction.LEFT_MOUSE);
                        SetAction(InputActionEnum.LEFT_MOUSE);
                    }

                    break;
                }
            }
        }

        private void BlockAndHideMouse(bool block)
        {
            Cursor.visible = !block;
            m_mouseInteractBlocker.SetBlock(block);
        }
    }

    public enum InputActionEnum
    {
        NONE,
        CANCEL,
        LEFT_MOUSE,
        SUBMIT,
        NOTE_SCREEN,
        LOG_SCREEN,
        TAB_NAVIGATION_LEFT,
        TAB_NAVIGATION_RIGHT,
    }
}