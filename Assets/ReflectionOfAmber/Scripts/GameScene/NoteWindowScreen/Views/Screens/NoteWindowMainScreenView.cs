using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Misc;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using ReflectionOfAmber.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views.Screens
{
    public class NoteWindowMainScreenView : NoteWindowScreenBase
    {
        [SerializeField] 
        private ButtonExt m_continueButton;
        [SerializeField] 
        private ButtonExt m_exitButton;
        
        private NoteWindowScreenPopup m_noteWindowScreenPopup;
        private  ConfirmScreen m_confirmScreen;
        private  SceneService m_sceneService;
        
        public override NoteWindowScreensEnum NoteWindowScreensEnum => NoteWindowScreensEnum.MAIN_SCREEN;
        public override Selectable GetFirstSelectable()
        {
            return m_continueButton;
        }

        [Inject]
        public void Construct(
            NoteWindowScreenPopup noteWindowScreenPopup,
            ConfirmScreen confirmScreen, 
            SceneService sceneService)
        {
            m_noteWindowScreenPopup = noteWindowScreenPopup;
            m_confirmScreen = confirmScreen;
            m_sceneService = sceneService;
        }
        
        // public void Init(
        //     NoteWindowScreenPopup noteWindowScreenPopup,
        //     ConfirmScreen confirmScreen, 
        //     SceneService sceneService)
        // {
        //     m_noteWindowScreenPopup = noteWindowScreenPopup;
        //     m_confirmScreen = confirmScreen;
        //     m_sceneService = sceneService;
        // }
        public override void Open()
        {
            base.Open();
            
            FocusUIManager.Instance.JumpSelectionToObject(GetFirstSelectable());
        }
        
        private void Awake()
        {
            m_continueButton.onClick.AddListener(ClickContinueButton);
            m_exitButton.onClick.AddListener(ClickExitButton);
        }

        private void ClickContinueButton()
        {
            m_noteWindowScreenPopup.Hide();
        }

        private void ClickExitButton()
        {
            m_confirmScreen.Check(ExitConfirmHandler, TranslatorKeys.CONFIRM_EXIT);

        }
        
        private void ExitConfirmHandler(bool isConfirm)
        {
            if(!isConfirm)
            {
                return;
            }
            GameModel.IsGamePlaying = false;
            m_sceneService.LoadMainMenuScene();
        }
    }
}