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
            // Variant commands have already built Addressables from saved assets.
            // Do not start an unawaited localization download during the player build.
            if (GameBuildCommands.IsBuilding)
                return;

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
