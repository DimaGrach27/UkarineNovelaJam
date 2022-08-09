namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionOpenEye : IActionScreen
    {
        public ActionOpenEye(ScreenPartsServiceFacade screenPartsServiceFacade)
        {
            _screenPartsServiceFacade = screenPartsServiceFacade;
        }

        private readonly ScreenPartsServiceFacade _screenPartsServiceFacade;
        
        public void Action()
        {
            OpenEyeAnimation.Inst.PlayOpenEye(OnDoneOpenAnima);
        }

        private void OnDoneOpenAnima()
        {
            _screenPartsServiceFacade.PlatNextPart();
        }
        
        public ActionType ActionType => ActionType.OPEN_EYE_ANIMA;
    }
    
    public class ActionPrepareOpenEye : IActionScreen
    {

        public void Action()
        {
            OpenEyeAnimation.Inst.PrepareEye();
        }
        
        public ActionType ActionType => ActionType.PREPARE_OPEN_EYE_ANIMA;
    }
}
