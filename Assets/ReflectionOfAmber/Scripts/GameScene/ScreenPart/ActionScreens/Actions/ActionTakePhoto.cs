namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionTakePhoto : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.CameraActionService.TakePhotoAction();
        }

        public override ActionType ActionType => ActionType.TAKE_PHOTO;
    }
}