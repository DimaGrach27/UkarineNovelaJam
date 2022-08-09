using ReflectionOfAmber.Scripts.GameScene.ChooseWindow.CameraAction;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionTakePhoto : IActionScreen
    {
        public ActionTakePhoto(CameraActionService actionService)
        {
            _cameraActionService = actionService;
        }

        private readonly CameraActionService _cameraActionService;
        
        public void Action()
        {
            _cameraActionService.TakePhotoAction();
        }

        public ActionType ActionType => ActionType.TAKE_PHOTO;
    }
}