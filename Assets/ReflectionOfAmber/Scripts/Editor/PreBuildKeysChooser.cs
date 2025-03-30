using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.Editor
{
    public class PreBuildKeysChooser : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;
        
        public void OnPreprocessBuild(BuildReport report)
        {
            throw new System.NotImplementedException();
        }

        private void ChangeAnalyticKeys(BuildReport report)
        {
            GameAnalyticsSDK.Setup.Settings settings = Resources.Load<GameAnalyticsSDK.Setup.Settings>("GameAnalytics");
#if UNITY_EDITOR_OSX
            Debug.Log("Changing analytic keys in OSX");
#endif
        }
    }
}