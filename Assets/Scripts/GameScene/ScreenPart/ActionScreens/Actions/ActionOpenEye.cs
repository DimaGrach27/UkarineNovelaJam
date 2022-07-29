namespace GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionOpenEye : IActionScreen
    {
        public ActionOpenEye(ScreenPartsService screenPartsService)
        {
            _screenPartsService = screenPartsService;
        }

        private readonly ScreenPartsService _screenPartsService;
        
        public void Action()
        {
            OpenEyeAnimation.Inst.PlayOpenEye(OnDoneOpenAnima);
        }

        private void OnDoneOpenAnima()
        {
            _screenPartsService.ShowNextPart();
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
