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
        private readonly GameResourcesService m_gameResourcesService;
        
        public event Action OnReady;

        private Dictionary<InputActionID, InputAction> m_Actions = new ();

        [Inject]
        public PlayerInputHandler(PlayerInput playerInput, GameResourcesService gameResourcesService)
        {
            m_PlayerInput = playerInput;
            m_gameResourcesService = gameResourcesService;
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
            
            OnReady?.Invoke();
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

            DeviceType deviceType = DeviceType.KeyboardAndMouse;
            switch (layoutName)
            {
                
            }
            foreach (var uiHintData in m_gameResourcesService.UiInputHintsConfig.GetUiHintData())
            {
                
            }

            return null;
        }

        public string CurrentControlScheme()
        {
            return m_PlayerInput.currentControlScheme;
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
    }

    public enum DeviceType
    {
        KeyboardAndMouse,
        XBOX,
        PS,
        DefaultGamepad,
    }

    public enum KeyCodePath
    {
        //Keyboard
        Q = 1,
        E = 2,
        L = 3,
        Enter = 4,
        Escape = 5,
        
        //Gamepad
        LeftShoulder = 6,
        RightShoulder = 7,
        ButtonEast = 8,
        ButtonWest = 9,
        ButtonNorth = 10,
        ButtonSouth = 11,
        Start = 12,
    }
}