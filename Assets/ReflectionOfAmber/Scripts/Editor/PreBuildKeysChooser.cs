#define TEST_KEY

using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.Editor
{
    public class PreBuildKeysChooser : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;
        
#if GAME_DEMO && GAME_FINAL
        private const string GameKeyWin = "a54b960045b8944a04310bd2639d5f0c";
        private const string SecretKeyWin = "6b337b9efcd317e8b489015ced0742d83a9862bd";
        
        private const string GameKeyOSX = "c7630c3b5f6d95d352cd2a0abff87af1";
        private const string SecretKeyOSX = "08ca02f66186abda3f39312f22456d540ae0f4da";
#elif GAME_FINAL && !GAME_DEMO
        private const string GameKeyWin = "b6af346853a69c1aafe4d09cd849d047";
        private const string SecretKeyWin = "2932f438d66c6137e10ade0ecd532a9ab1d43592";
        
        private const string GameKeyOSX = "45e84645fd546dd10c4ac7c500deba9b";
        private const string SecretKeyOSX = "1e867e0e96638460285bcd82e392de0e61016038";

#else
        private const string GameKeyWin = "cddccd5a3bda77c6c7d8f2c559fe4223";
        private const string SecretKeyWin = "de3649a8c583bbe852521c6a2c6cca43fb490bdf";
        
        private const string GameKeyOSX = "b92bb7f27519cedeae17197483b4a357";
        private const string SecretKeyOSX = "9bf51f6681794020c19d221fd9c998d63f8132ca";
#endif
        
        public void OnPreprocessBuild(BuildReport report)
        {
            ChangeAnalyticKeys(report);
        }

        private void ChangeAnalyticKeys(BuildReport report)
        {
            GameAnalyticsSDK.Setup.Settings settings = Resources.Load<GameAnalyticsSDK.Setup.Settings>("GameAnalytics/Settings");
#if UNITY_EDITOR_OSX
            Debug.Log("Changing analytic keys in OSX");
#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            Debug.Log("Changing analytic keys for WIN");
#endif
            settings.NewVersion = PlayerSettings.bundleVersion;
            
            int index = 0;
            foreach (var plat in settings.Platforms)
            {
                Debug.Log($"[GAME ANALYTICS] Platform = {plat}");

                Debug.Log($"[GAME ANALYTICS] GameKey = {settings.GetGameKey(index)}");
                Debug.Log($"[GAME ANALYTICS] GameSecret = {settings.GetSecretKey(index)}");
// #if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                if (plat == RuntimePlatform.WindowsPlayer)
                {
                    Debug.Log("Changing analytic keys for WIN");
                    GameAnalyticsSDK.Setup.Settings.UpdateKeys(index, GameKeyWin, SecretKeyWin);
                    Debug.Log($"[GAME ANALYTICS] GameKey = {GameKeyWin}");
                    Debug.Log($"[GAME ANALYTICS] GameSecret = {SecretKeyWin}");
                }
// #endif
                
// #if UNITY_EDITOR_OSX
                if (plat == RuntimePlatform.OSXPlayer)
                {
                    Debug.Log("Changing analytic keys in OSX");
                    GameAnalyticsSDK.Setup.Settings.UpdateKeys(index, GameKeyOSX, SecretKeyOSX);
                    Debug.Log($"[GAME ANALYTICS] GameKey = {GameKeyOSX}");
                    Debug.Log($"[GAME ANALYTICS] GameSecret = {SecretKeyOSX}");
                }
// #endif
                index++;
            }
        }
    }
}