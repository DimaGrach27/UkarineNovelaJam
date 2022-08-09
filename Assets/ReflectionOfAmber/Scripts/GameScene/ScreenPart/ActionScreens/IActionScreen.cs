namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens
{
    public interface IActionScreen
    {
        public void Action();
        public ActionType ActionType { get; }
    }
}