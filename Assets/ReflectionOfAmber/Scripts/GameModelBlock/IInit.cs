using System;

namespace ReflectionOfAmber.Scripts.GameModelBlock
{
    public interface IInit
    {
        public event Action OnReady;
        public void Init();
    }
}