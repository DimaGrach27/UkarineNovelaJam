using UnityEngine;

namespace ReflectionOfAmber.Scripts
{
    public static class GlobalConstant
    {
        public const int MAX_HEALTH = 4;
        
        public const float ANIMATION_DISSOLVE_DURATION = 0.75f;
        public const float DEFAULT_FADE_DURATION = 1.5f;
        public const float CAMERA_ACTION_FLASH_DURATION = 0.5f;

        public static readonly Color ColorWitheClear = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        
        public const char Koma = ',';
        public const char DubbleComa = '"';
        public const char SymbolR = '\r';
        public const char SymbolN = '\n';
        public const char SymbolT = '\t';
        public static string StringComa => $"{DubbleComa}{DubbleComa}";
        
#if GAME_DEMO
        public const string LAST_DEMO_SCENE_KEY = "scene_0_2";
        // public const string LAST_DEMO_SCENE_KEY = "scene_6_61";
#endif
    }
}