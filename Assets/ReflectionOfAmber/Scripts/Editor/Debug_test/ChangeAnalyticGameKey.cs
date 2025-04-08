#define TEST_KEY

using UnityEditor;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.Editor.Debug_test
{
    public class ChangeAnalyticGameKey
    {
#if TEST_KEY
        private const string GameKey = "cddccd5a3bda77c6c7d8f2c559fe4223";
        private const string SecretKey = "de3649a8c583bbe852521c6a2c6cca43fb490bdf";
#else   
        private const string GameKeyWin = "a54b960045b8944a04310bd2639d5f0c";
        private const string SecretKeyWin = "6b337b9efcd317e8b489015ced0742d83a9862bd";
#endif
        
        [MenuItem("ReflectionOfAmber/ChangeKeys")]
        private static void ChangeAnalyticKeys()
        {
            GameAnalyticsSDK.Setup.Settings settings = Resources.Load<GameAnalyticsSDK.Setup.Settings>("GameAnalytics/Settings");
#if UNITY_EDITOR_OSX
            Debug.Log("Changing analytic keys in OSX");
#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            Debug.Log("Changing analytic keys for WIN");
#endif
            settings.SetKeys(GameKey, SecretKey);
            settings.NewVersion = PlayerSettings.bundleVersion;
            
            Debug.Log($"[GAME ANALYTICS] GameKey = {GameKey}");
            Debug.Log($"[GAME ANALYTICS] GameSecret = {SecretKey}");
            
            int index = 0;
            foreach (var plat in settings.Platforms)
            {
                Debug.Log($"[GAME ANALYTICS] Platform = {plat}");

                Debug.Log($"[GAME ANALYTICS] GameKey = {settings.GetGameKey(index)}");
                Debug.Log($"[GAME ANALYTICS] GameSecret = {settings.GetSecretKey(index)}");
                
                if (plat == RuntimePlatform.WindowsPlayer)
                {
                    GameAnalyticsSDK.Setup.Settings.UpdateKeys(index, GameKey, SecretKey);
                }
                index++;
            }
        }
    }
}