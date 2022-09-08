using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views.Screens;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GlobalProject;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Services
{
    public class NoteWindowInvestigationScreenService
    {
        private readonly ScreenPartsServiceFacade _screenPartsServiceFacade;
        private readonly ConfirmScreen _confirmScreen;
        private readonly string[] _nextScenes;
        
        private int _selectIndex = -1;

        [Inject]
        public NoteWindowInvestigationScreenService(NoteWindowInvestigationScreen noteWindowInvestigationScreen, 
            ScreenPartsServiceFacade screenPartsServiceFacade,
            ConfirmScreen confirmScreen)
        {
            _screenPartsServiceFacade = screenPartsServiceFacade;
            _confirmScreen = confirmScreen;
            
            _nextScenes = new string[3];

            _nextScenes[0] = "scene_3_164_3";
            _nextScenes[1] = "scene_3_164_1";
            _nextScenes[2] = "scene_3_164_2";
            
            noteWindowInvestigationScreen.OnChoose += OnChooseClick;
        }
        

        private void OnChooseClick(int index)
        {
            _selectIndex = index;
            string description = "Ви впевнені, що саме цей підозрюваний являється злодієм?\n" +
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