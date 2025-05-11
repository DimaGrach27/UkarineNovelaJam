using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ReflectionOfAmber.Scripts.Editor
{
    public class PreBuildManager : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;
        
        private const string LOCALIZATION_ASSET_KEY = "Localization";
        public void OnPreprocessBuild(BuildReport report)
        {
            Addressables.LoadAssetAsync<LocalizationConfig>(LOCALIZATION_ASSET_KEY).Completed += handle =>
            {
                Debug.Log("LOCALIZATION CONFIG TRY TO UPDATE");
                handle.Result.UpdateLocalization();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            };
        }
    }
}