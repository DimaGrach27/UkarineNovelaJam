using System;

namespace ReflectionOfAmber.Scripts.GameScene
{
    public static class GlobalEvent
    {
        public static event Action<CallKeyType> OnCallType;
        public static void CallType(CallKeyType type) => OnCallType?.Invoke(type);
    }

    public enum CallKeyType
    {
        NONE = 0,
        OPEN_SETTINGS = 1,
        GAME_PAUSE_MENU = 2
    }
}