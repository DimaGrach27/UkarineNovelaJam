using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build.DataBuilders;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.Build;
using UnityEditor.Build.Profile;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.Editor
{
    [InitializeOnLoad]
    public static class GameBuildCommands
    {
        private const string PendingKey = "ReflectionOfAmber.PendingBuild";
        private const string DemoDefine = "GAME_DEMO";
        public static bool IsBuilding { get; private set; }

        [Serializable]
        private class BuildRequest
        {
            public bool demo;
            public int target;
            public string output;
            public string profileGuid;
        }

        static GameBuildCommands()
        {
            EditorApplication.update += ResumeBuild;
        }

        [MenuItem("Build/Build Demo")]
        public static void BuildDemo() => QueueBuild(true);

        [MenuItem("Build/Build Full")]
        public static void BuildFull() => QueueBuild(false);

        [MenuItem("Build/Build Demo", true)]
        [MenuItem("Build/Build Full", true)]
        private static bool CanBuild() => !EditorApplication.isPlayingOrWillChangePlaymode
            && !EditorApplication.isCompiling && !BuildPipeline.isBuildingPlayer
            && !IsBuilding && string.IsNullOrEmpty(SessionState.GetString(PendingKey, ""));

        [MenuItem("Build/Cancel Pending Build")]
        private static void CancelPendingBuild()
        {
            SessionState.EraseString(PendingKey);
            Debug.Log("Pending game build cancelled. Current scripting defines are retained.");
        }

        private static void QueueBuild(bool demo)
        {
            try
            {
                var profile = BuildProfile.GetActiveBuildProfile();

                var target = EditorUserBuildSettings.activeBuildTarget;
                if (target != BuildTarget.StandaloneWindows && target != BuildTarget.StandaloneWindows64
                    && target != BuildTarget.StandaloneOSX && target != BuildTarget.StandaloneLinux64)
                    throw new BuildFailedException("Select Windows, macOS or Linux in Build Profiles before building.");

                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    return;

                ValidateSettings();
                if (!(profile != null ? profile.GetScenesForBuild() : EditorBuildSettings.scenes).Any(scene => scene.enabled))
                    throw new BuildFailedException("No enabled scenes in Build Profiles.");

                var variant = demo ? "Demo" : "Full";
                var extension = target == BuildTarget.StandaloneOSX ? ".app"
                    : target == BuildTarget.StandaloneLinux64 ? "" : ".exe";
                var request = new BuildRequest
                {
                    demo = demo,
                    target = (int)target,
                    profileGuid = GetActiveProfileGuid(),
                    output = Path.GetFullPath(Path.Combine("Builds", variant, target.ToString(),
                        profile != null ? SafeFolderName(profile.name) + "-" + GetActiveProfileGuid().Substring(0, 8) : "",
                        "ReflectionOfAmber" + (demo ? "Demo" : "") + extension))
                };
                var namedTarget = NamedBuildTarget.FromBuildTargetGroup(BuildPipeline.GetBuildTargetGroup(target));
                var current = PlayerSettings.GetScriptingDefineSymbols(namedTarget);
                var defines = current.Split(';').Where(value => !string.IsNullOrWhiteSpace(value))
                    .Select(value => value.Trim()).Where(value => value != DemoDefine).ToList();
                if (demo && profile == null)
                    defines.Add(DemoDefine);
                var updated = string.Join(";", defines.Distinct());

                // SessionState survives the domain reload caused by changing scripting defines.
                SessionState.SetString(PendingKey, JsonUtility.ToJson(request));
                Debug.Log($"Queued {variant} build: {request.output}. Waiting for script compilation if needed.");
                if (current != updated)
                    PlayerSettings.SetScriptingDefineSymbols(namedTarget, updated);
                if (profile != null)
                {
                    // Profile defines are additive: remove GAME_DEMO from effective PlayerSettings
                    // and keep this variant switch in the active profile's own define list.
                    var profileDefines = profile.scriptingDefines.Where(value => value != DemoDefine).ToList();
                    if (demo)
                        profileDefines.Add(DemoDefine);
                    profile.scriptingDefines = profileDefines.Distinct().ToArray();
                }
                AssetDatabase.SaveAssets();
            }
            catch (Exception exception)
            {
                SessionState.EraseString(PendingKey);
                Debug.LogException(exception);
            }
        }

        private static void ResumeBuild()
        {
            var pending = SessionState.GetString(PendingKey, "");
            if (string.IsNullOrEmpty(pending) || IsBuilding || EditorApplication.isCompiling
                || EditorApplication.isUpdating || EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            if (EditorUtility.scriptCompilationFailed)
            {
                CancelPendingBuild();
                Debug.LogError("Game build cancelled because scripts failed to compile.");
                return;
            }

            var request = JsonUtility.FromJson<BuildRequest>(pending);
            if (EditorUserBuildSettings.activeBuildTarget != (BuildTarget)request.target
                || GetActiveProfileGuid() != request.profileGuid)
            {
                CancelPendingBuild();
                Debug.LogError("Build target or profile changed while waiting. Run the build command again.");
                return;
            }
#if GAME_DEMO
            const bool compiledAsDemo = true;
#else
            const bool compiledAsDemo = false;
#endif
            // Do not build from the old assembly before Unity starts the requested reload.
            if (compiledAsDemo != request.demo)
                return;

            SessionState.EraseString(PendingKey);
            try
            {
                Build(request);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        private static string GetActiveProfileGuid()
        {
            var profile = BuildProfile.GetActiveBuildProfile();
            return profile == null ? "" : AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(profile));
        }

        private static string SafeFolderName(string name)
        {
            return string.Concat(name.Select(character => Path.GetInvalidFileNameChars().Contains(character) ? '_' : character));
        }

        private static AddressableAssetSettings ValidateSettings()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
                throw new BuildFailedException("Addressables settings are missing.");
            foreach (var name in new[] { "MainStory", "Prologue", "Default Local Group" })
            {
                if (settings.FindGroup(name)?.GetSchema<BundledAssetGroupSchema>() == null)
                    throw new BuildFailedException($"Addressables group '{name}' or its bundled schema is missing.");
            }
            if (!settings.DataBuilders.Any(builder => builder is BuildScriptPackedMode))
                throw new BuildFailedException("Addressables Default Build Script (BuildScriptPackedMode) is missing.");
            return settings;
        }

        private static void Build(BuildRequest request)
        {
            var settings = ValidateSettings();
            var originalBuildOption = settings.BuildAddressablesWithPlayerBuild;
            var originalBuilder = settings.ActivePlayerDataBuilderIndex;
            var schemas = new[] { "MainStory", "Prologue", "Default Local Group" }
                .Select(name => settings.FindGroup(name).GetSchema<BundledAssetGroupSchema>()).ToArray();
            var originalInclusion = schemas.Select(schema => schema.IncludeInBuild).ToArray();
            IsBuilding = true;
            try
            {
                schemas[0].IncludeInBuild = !request.demo;
                schemas[1].IncludeInBuild = true;
                schemas[2].IncludeInBuild = true;
                settings.ActivePlayerDataBuilderIndex = settings.DataBuilders.FindIndex(builder => builder is BuildScriptPackedMode);
                // Build content explicitly and stop on errors, then reuse it for the player build.
                settings.BuildAddressablesWithPlayerBuild = AddressableAssetSettings.PlayerBuildOption.DoNotBuildWithPlayer;
                AssetDatabase.SaveAssets();
                AddressableAssetSettings.BuildPlayerContent(out var contentResult);
                if (contentResult == null || !string.IsNullOrEmpty(contentResult.Error))
                    throw new BuildFailedException("Addressables build failed: " + contentResult?.Error);

                Directory.CreateDirectory(Path.GetDirectoryName(request.output));
                var profile = BuildProfile.GetActiveBuildProfile();
                var report = profile != null
                    ? BuildPipeline.BuildPlayer(new BuildPlayerWithProfileOptions
                    {
                        buildProfile = profile,
                        locationPathName = request.output,
                        options = BuildOptions.None
                    })
                    : BuildPipeline.BuildPlayer(new BuildPlayerOptions
                {
                    scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray(),
                    target = (BuildTarget)request.target,
                    locationPathName = request.output,
                    options = BuildOptions.None
                });
                if (report == null || report.summary.result != BuildResult.Succeeded)
                    throw new BuildFailedException("Player build did not succeed. See the Console and build report.");
                Debug.Log($"{(request.demo ? "Demo" : "Full")} build succeeded: {request.output}");
                EditorUtility.RevealInFinder(request.output);
            }
            finally
            {
                for (var index = 0; index < schemas.Length; index++)
                    schemas[index].IncludeInBuild = originalInclusion[index];
                settings.BuildAddressablesWithPlayerBuild = originalBuildOption;
                settings.ActivePlayerDataBuilderIndex = originalBuilder;
                IsBuilding = false;
                AssetDatabase.SaveAssets();
            }
        }
    }
}
