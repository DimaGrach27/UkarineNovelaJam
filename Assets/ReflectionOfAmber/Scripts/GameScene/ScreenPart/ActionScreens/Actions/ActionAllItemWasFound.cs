namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionAllItemWasFound : IActionScreen
    {
        public ActionAllItemWasFound(GamePlayCanvas gamePlayCanvas)
        {
            _allItemWasFoundUiView = gamePlayCanvas.GetComponentInChildren<AllItemWasFoundUiView>();
        }
        
        private readonly AllItemWasFoundUiView _allItemWasFoundUiView;
        
        public void Action()
        {
            _allItemWasFoundUiView.Show();
        }

        public ActionType ActionType => ActionType.ALL_ITEM_WAS_FOUND;
    }
}