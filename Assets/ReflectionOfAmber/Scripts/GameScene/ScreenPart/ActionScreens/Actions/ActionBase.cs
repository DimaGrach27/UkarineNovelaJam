namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public abstract class ActionBase : IActionScreen
    {
        public void InitService(ActionScreenService actionScreenService)
        {
            ActionScreenService = actionScreenService;
        }
        
        protected ActionScreenService ActionScreenService;
        
        public abstract void Action();
        public abstract ActionType ActionType { get; }
    }
}