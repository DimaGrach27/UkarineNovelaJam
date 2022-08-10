using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.MainMenu;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindow
{
    public class NoteService
    {
        private readonly ScreenPartsServiceFacade _screenPartsServiceFacade;
        private readonly ConfirmScreen _confirmScreen;
        private readonly string[] _nextScenes;
        
        private int _selectIndex = -1;

        [Inject]
        public NoteService(GamePlayCanvas gamePlayCanvas, 
            ScreenPartsServiceFacade screenPartsServiceFacade,
            ConfirmScreen confirmScreen)
        {
            _screenPartsServiceFacade = screenPartsServiceFacade;
            _confirmScreen = confirmScreen;
            
            _nextScenes = new string[3];

            _nextScenes[0] = "scene_3_164_3";
            _nextScenes[1] = "scene_3_164_1";
            _nextScenes[2] = "scene_3_164_2";
            
            var noteWindowUIView = gamePlayCanvas.GetComponentInChildren<NoteWindowUIView>();
            noteWindowUIView.OnChoose += OnChooseClick;
        }

        private void OnChooseClick(int index)
        {
            _selectIndex = index;
            string description = "Ви впевнені, що саме цей підозрюваний являється злодієм? " +
                                 "Помилка призведе до необачних дій...";
            
            _confirmScreen.Check(ConfirmAction, description);
        }

        private void ConfirmAction(bool isConfirm)
        {
            if (isConfirm)
            {
                SaveService.SetStatusValue(StatusEnum.CHOOSE_WAS_PICK, true);
                _screenPartsServiceFacade.PlayNextScene(_nextScenes[_selectIndex]);
            }

            _selectIndex = -1;
        }
    }
}