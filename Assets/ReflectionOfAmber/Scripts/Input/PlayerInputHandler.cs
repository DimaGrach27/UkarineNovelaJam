using System;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace ReflectionOfAmber.Scripts.Input
{
    public class PlayerInputHandler : IInit
    {
        public static PlayerInputHandler Instance;
        
        private readonly PlayerInput m_PlayerInput;
        private readonly GameResourcesService m_GameResourcesService;

        public event Action OnReady;

        private readonly Dictionary<InputActionID, InputAction> m_Actions = new ();
        private readonly List<IDeviceChangeListener> m_DeviceChangeListeners = new ();

        private DeviceType m_CurrentDeviceType;

        [Inject]
        public PlayerInputHandler(PlayerInput playerInput, GameResourcesService gameResourcesService)
        {
            m_PlayerInput = playerInput;
            m_GameResourcesService = gameResourcesService;
        }
        
        public void Init()
        {
            if (Instance is not null)
            {
                throw new Exception("Only one instance of PlayerInputManager");
            }
            
            Instance = this;
            
            if (m_PlayerInput is null)
            {
                throw new Exception("PlayerInput is null");
            }

            foreach (var inputAction in m_PlayerInput.actions)
            {
                Enum.TryParse(inputAction.name, true, out InputActionID inputActionID);
                m_Actions[inputActionID] = inputAction;
            }

            SetupInputEvents();
            
            OnReady?.Invoke();
        }
        
        public DeviceType GetCurrentDeviceName()
        {
            return m_CurrentDeviceType;
        }
        
        public string GetButtonHint(InputActionID action)
        {
            if (!m_Actions.TryGetValue(action, out InputAction inputAction))
            {
                throw new Exception($"Action not found. Invalid action type {action}!");
            }

            string controlPath;
            string layoutName;
            int binding = inputAction.GetBindingIndex(m_PlayerInput.currentControlScheme);
            string displayString = inputAction.GetBindingDisplayString(binding, out layoutName, out controlPath);

            return controlPath;
        }

        public Sprite GetIconHint(InputActionID action)
        {
            if (!m_Actions.TryGetValue(action, out InputAction inputAction))
            {
                throw new Exception($"Action not found. Invalid action type {action}!");
            }
            
            int binding = inputAction.GetBindingIndex(m_PlayerInput.currentControlScheme);
            inputAction.GetBindingDisplayString(binding, out string layoutName, out string controlPath);

            foreach (var uiHintData in m_GameResourcesService.UiInputHintsConfig.GetUiHintData())
            {
                if (uiHintData.DeviceType == m_CurrentDeviceType)
                {
                    foreach (var keybindingData in uiHintData.KeybindingData)
                    {
                        if (keybindingData.ActionID.ToString()
                            .Equals(controlPath, StringComparison.CurrentCultureIgnoreCase))
                        {
                            return keybindingData.Icon;
                        }
                    }
                }
            }

            return null;
        }

        public void Subscribe(IDeviceChangeListener listener)
        {
            m_DeviceChangeListeners.Add(listener);
        }

        public void Unsubscribe(IDeviceChangeListener listener)
        {
            m_DeviceChangeListeners.Remove(listener);
        }
        
        private void UpdateDeviceType()
        {
            string deviceName = m_PlayerInput.devices[0].displayName;
            if (deviceName.Contains("XBOX", StringComparison.CurrentCultureIgnoreCase))
            {
                m_CurrentDeviceType = DeviceType.XBOX;
            }
            else if (deviceName.Contains("PS4", StringComparison.CurrentCultureIgnoreCase))
            {
                m_CurrentDeviceType = DeviceType.PS4;
            }
            else if (deviceName.Contains("PS5", StringComparison.CurrentCultureIgnoreCase))
            {
                m_CurrentDeviceType = DeviceType.PS5;
            }
            else if (deviceName.Contains("Controller", StringComparison.CurrentCultureIgnoreCase))
            {
                m_CurrentDeviceType = DeviceType.DefaultGamepad;
            }
            else
            {
                m_CurrentDeviceType = DeviceType.KeyboardAndMouse;
            }
        }
        
        private void SetupInputEvents()
        {
            InputActionMap uiActionMap = m_PlayerInput.actions.FindActionMap("UI");
            
            foreach (var inputAction in uiActionMap.actions)
            {
                if (inputAction != null)
                {
                    inputAction.performed += InputActionPerformed;
                }
            }
        }

        private void InputActionPerformed(InputAction.CallbackContext obj)
        {
            DeviceType deviceTypeTemp = m_CurrentDeviceType;
            UpdateDeviceType();

            if (m_CurrentDeviceType != deviceTypeTemp)
            {
                foreach (var deviceChangeListener in m_DeviceChangeListeners)
                {
                    deviceChangeListener.OnDeviceChangedHandler();
                }
            }
        }
    }

    public enum InputActionID
    {
        None,
        Submit,
        Cancel,
        Navigate,
        LogScreen,
        NoteScreen,
        TabNavigation_Right,
        TabNavigation_Left,
        CameraAction,
    }

    public enum DeviceType
    {
        Unknown,
        KeyboardAndMouse,
        XBOX,
        PS4,
        PS5,
        DefaultGamepad,
    }

    //Last 14
    public enum KeyCodePath
    {
        //Keyboard
        Q = 1,
        E = 2,
        L = 3,
        Enter = 4,
        Escape = 5,
        F = 14,
        
        //Gamepad
        LeftShoulder = 6,
        RightShoulder = 7,
        ButtonEast = 8,
        ButtonWest = 9,
        ButtonNorth = 10,
        ButtonSouth = 11,
        Start = 12,
        RightTrigger = 13,
    }
}