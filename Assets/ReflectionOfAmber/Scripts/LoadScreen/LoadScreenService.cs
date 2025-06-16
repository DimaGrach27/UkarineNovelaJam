using System;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.BgScreen;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using ReflectionOfAmber.Scripts.Input;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.LoadScreen
{
    public class LoadScreenService : IDisposable, IInputListener
    {
        private readonly LoadScreenView m_loadScreenView;
        private readonly ConfirmScreen m_confirmScreen;
        private readonly SceneService m_sceneService;
        private readonly InputService m_inputService;

        public event Action OnCloseClick;
        
        [Inject]
        public LoadScreenService(LoadScreenView mLoadScreenView,
            ConfirmScreen mConfirmScreen,
            SceneService mSceneService,
            InputService inputService)
        {
            m_loadScreenView = mLoadScreenView;
            m_confirmScreen = mConfirmScreen;
            m_sceneService = mSceneService;
            m_inputService = inputService;

            m_loadScreenView.OnCLickButton += OnClickToCardHandler;
            m_loadScreenView.OnCloseClick += OnCloseClickHandler;

            m_hasSaveArray = new bool[m_loadScreenView.Count];
        }

        private readonly bool[] m_hasSaveArray;
        private int m_confirmIndex = -1;

        public void Open()
        {
            m_loadScreenView.Open();
            
            for (int i = 0; i < m_loadScreenView.Count; i++)
            {
                if (SaveService.TryGetSaveGame(i, out SaveFile saveFile))
                {
                    m_hasSaveArray[i] = true;
                    BgEnum bgEnum = (BgEnum)saveFile.currentBg;
                    Sprite spriteBg = GameModel.GetBg(bgEnum);
                    string saveText = $"{TranslatorService.GetText(TranslatorKeys.SAVE_ON_CARD)} {i + 1}";
                    m_loadScreenView.UpdateElement(i, spriteBg, true, saveText);
                }
                else
                {
                    m_hasSaveArray[i] = false;
                    m_loadScreenView.UpdateElement(i, null, false, TranslatorService.GetText(TranslatorKeys.EMPTY));
                }
            }
            
            m_inputService.ForceRedirectInput(this);
        }

        private void OnClickToCardHandler(int index)
        {
            if(!m_hasSaveArray[index]) return;
            m_confirmIndex = index;
            m_confirmScreen.Check(ConfirmLoad, TranslatorKeys.CONFIRM_NEW_GAME);
        }

        private void ConfirmLoad(bool isConfirm)
        {
            if (isConfirm)
            {
                SaveService.SetLoadGame(m_confirmIndex);
                m_sceneService.LoadGameScene();
            }

            m_confirmIndex = -1;
        }

        private void OnCloseClickHandler()
        {
            m_inputService.RemoveForceRedirected(this);
            OnCloseClick?.Invoke();
        }
        
        public void Dispose()
        {
            m_loadScreenView.OnCLickButton -= OnClickToCardHandler;
            m_loadScreenView.OnCloseClick -= OnCloseClickHandler;
        }

        public void OnInputAction(InputActionEnum inputActionEnum)
        {
            if (inputActionEnum == InputActionEnum.CANCEL)
            { 
                m_loadScreenView.FadeOutWindow();
                OnCloseClickHandler();
            }
        }

        public bool ShouldReceiveInput { get; set; }
    }
}