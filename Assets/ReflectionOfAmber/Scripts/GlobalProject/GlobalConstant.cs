using UnityEngine;

namespace ReflectionOfAmber.Scripts.GlobalProject
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
        // public const string LAST_DEMO_SCENE_KEY = "scene_0_2";
        // scene_6_61 - scene where Vilhanka went to trap
        // scene_2_30 - scene where Vilhanka ended looking to the body
        public const string LAST_DEMO_SCENE_KEY = "scene_2_30";
#endif
        
            //for localization
            public const string Id = "1ym156FGXOVntcnxxydhQx8hRfOE5EzgpoxMXq53fCbc";
            public const string ExportFormat = "export?format=tsv";
            public const string GidScenario = "327397956"; // 0 - is old scenario, 327397956 - new scenario (parsed)
            public const string GidOtherText = "208247162";
            // private static readonly string ScenarioURL = $"https://docs.google.com/spreadsheets/d/{Id}/{ExportFormat}";
            public static readonly string ScenarioURL = $"https://docs.google.com/spreadsheets/d/{Id}/{ExportFormat}&id={Id}&gid={GidScenario}";
            public static readonly string OtherTextURL = $"https://docs.google.com/spreadsheets/d/{Id}/{ExportFormat}&id={Id}&gid={GidOtherText}";

    }
}