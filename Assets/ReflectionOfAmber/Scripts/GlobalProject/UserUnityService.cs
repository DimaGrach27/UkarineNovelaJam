using System;
using ReflectionOfAmber.Scripts.GameModelBlock;
using Unity.Services.Core;

namespace ReflectionOfAmber.Scripts.GlobalProject
{
    public class UserUnityService : IInit
    {
        public event Action OnReady;
        public async void Init()
        {
            await UnityServices.InitializeAsync();
            OnReady?.Invoke();
        }
    }
}