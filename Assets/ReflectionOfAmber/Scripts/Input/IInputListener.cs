namespace ReflectionOfAmber.Scripts.Input
{
    public interface IInputListener
    {
        public void OnInputAction(InputActionEnum inputActionEnum);
        
        public bool ShouldReceiveInput { get; set; }
    }
}