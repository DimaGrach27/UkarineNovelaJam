namespace ReflectionOfAmber.Scripts.Input
{
    public interface IInputListener
    {
        public void OnInputAction(InputAction inputAction);
        
        public bool ShouldReceiveInput { get; set; }
    }
}