namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionOpenEye : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.OpenEyeAnimation.PlayOpenEye(OnDoneOpenAnima);
        }

        private void OnDoneOpenAnima()
        {
            ActionScreenService.ScreenPartsServiceFacade.PlatNextPart();
        }
        
        public override ActionType ActionType => ActionType.OPEN_EYE_ANIMA;
    }
    
    public class ActionPrepareOpenEye : ActionBase
    {

        public override void Action()
        {
            ActionScreenService.OpenEyeAnimation.PrepareEye();
        }
        
        public override ActionType ActionType => ActionType.PREPARE_OPEN_EYE_ANIMA;
    }
}
