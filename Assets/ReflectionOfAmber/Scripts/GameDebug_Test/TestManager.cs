using System;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.Input;
using ReflectionOfAmber.Scripts.PreInitScene;
using ReflectionOfAmber.Scripts.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ReflectionOfAmber.Scripts.GameDebug_Test
{
    public class TestManager : MonoBehaviour, IInputListener
    {
        [SerializeField] 
        private MouseInteractBlocker mouseInteractBlocker;
        
        [SerializeField]
        private PlayerInput m_playerInput;
        
        private PlayerInputHandler m_PlayerInput;
        
        private InputService m_InputService;
        
        private GameResourcesService m_GameResourcesService;

        public string Device;
        
        private GameIniter m_GameIniter;
        

        [SerializeField] 
        private UiButton[] m_Buttons;

        private void Awake()
        {
            m_GameResourcesService = new GameResourcesService();
            m_InputService = new InputService(mouseInteractBlocker);
            m_PlayerInput = new PlayerInputHandler(m_playerInput, m_GameResourcesService);

            m_GameIniter = new GameIniter(new List<IInit>()
            {
                m_GameResourcesService,
                m_InputService,
                m_PlayerInput
            });
            
            m_GameIniter.Initialize();

            m_GameResourcesService.OnReady += InitInput;
            m_GameResourcesService.Init();
            m_PlayerInput.Init();

            m_InputService.AddListener(this);
        }

        private void InitInput()
        {
            m_GameResourcesService.OnReady -= InitInput;
            m_InputService.OnReady += InitPlayerInput;
            m_InputService.Init();
        }
        
        private void InitPlayerInput()
        {
            m_InputService.OnReady -= InitPlayerInput;
            
        }

        private void Start()
        {
            for (int i = 0; i < m_Buttons.Length; i++)
            {
                UiButton uiButton = m_Buttons[i];

                uiButton.DisplayedText = PlayerInputHandler.Instance.GetButtonHint(uiButton.InputActionID);
                
                m_Buttons[i] = uiButton;
            }
        }

        private void Update()
        {
            m_InputService?.Tick();
            
            // HintLeft = m_PlayerInput.GetButtonHint(InputActionID.TabNavigation_Left);
            // HintRight = m_PlayerInput.GetButtonHint(InputActionID.TabNavigation_Right);
            
            for (int i = 0; i < m_Buttons.Length; i++)
            {
                UiButton uiButton = m_Buttons[i];

                uiButton.DisplayedText = PlayerInputHandler.Instance.GetButtonHint(uiButton.InputActionID);
                
                m_Buttons[i] = uiButton;
            }

            Device = PlayerInputHandler.Instance.CurrentControlScheme();
        }
        
        public void OnInputAction(InputActionEnum inputActionEnum)
        {

        }

        public bool ShouldReceiveInput { get; set; } = true;

        [Serializable]
        public struct UiButton
        {
            public string DisplayedText;
            public InputActionID InputActionID;
        }
    }
}