using System;

namespace ReflectionOfAmber.Scripts.GlobalProject
{
    public interface IInit
    {
        public event Action OnReady;
        public void Init();
    }
}