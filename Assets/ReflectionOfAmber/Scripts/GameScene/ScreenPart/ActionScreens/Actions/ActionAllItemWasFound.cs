namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionAllItemWasFound : ActionBase
    {
        private AllItemWasFoundUiView _allItemWasFoundUiView;
        private AllItemWasFoundUiView AllItemWasFoundUiView
        {
            get
            {
                if (_allItemWasFoundUiView == null)
                {
                    _allItemWasFoundUiView =
                        ActionScreenService.GamePlayCanvas.GetComponentInChildren<AllItemWasFoundUiView>();
                }

                return _allItemWasFoundUiView;
            }
        }
        
        public override void Action()
        {
            AllItemWasFoundUiView.Show();
        }

        public override ActionType ActionType => ActionType.ALL_ITEM_WAS_FOUND;
    }
}