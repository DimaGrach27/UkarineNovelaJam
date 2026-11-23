using System;
using System.Collections.Generic;
using System.Linq;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart.SpecialSO;
using UnityEditor;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Editor
{
    public static class SceneGraphData
    {
        public const string Root = "Assets/ReflectionOfAmber/Configs/Graphs";

        public sealed class SpecialTransition
        {
            public int OptionIndex;
            public SpecialScriptableObjectBase Asset;
            public string Field;
            public ScreenSceneScriptableObject Target;
        }

        public sealed class Issue
        {
            public ScreenSceneScriptableObject Scene;
            public int OptionIndex;
            public string Message;
            public UnityEngine.Object Context;
            public override string ToString() => $"{Scene.SceneKey} · Option {OptionIndex + 1}: {Message}";
        }

        public static ScreenSceneScriptableObject[] FindScenes() => AssetDatabase
            .FindAssets("t:ScreenSceneScriptableObject", new[] { Root })
            .Select(AssetDatabase.GUIDToAssetPath)
            .OrderBy(path => path, StringComparer.Ordinal)
            .Select(AssetDatabase.LoadAssetAtPath<ScreenSceneScriptableObject>).ToArray();

        public static bool SetTarget(ScreenSceneScriptableObject scene, int index, ScreenSceneScriptableObject target)
        {
            if (scene == null || scene == target || scene.nextScenes == null || index < 0 || index >= scene.nextScenes.Length)
                return false;
            if (scene.nextScenes[index] != null && scene.nextScenes[index].scene == target) return true;
            Undo.RegisterCompleteObjectUndo(scene, "Change scene transition");
            if (scene.nextScenes[index] == null) scene.nextScenes[index] = new NextScene();
            scene.nextScenes[index].scene = target;
            EditorUtility.SetDirty(scene);
            return true;
        }

        public static void AddOption(ScreenSceneScriptableObject scene)
        {
            if (scene == null) return;
            Undo.RegisterCompleteObjectUndo(scene, "Add scene option");
            var options = (scene.nextScenes ?? Array.Empty<NextScene>()).ToList();
            options.Add(new NextScene());
            scene.nextScenes = options.ToArray();
            EditorUtility.SetDirty(scene);
        }

        public static void RemoveOption(ScreenSceneScriptableObject scene, int index)
        {
            if (scene == null || scene.nextScenes == null || index < 0 || index >= scene.nextScenes.Length) return;
            Undo.RegisterCompleteObjectUndo(scene, "Remove scene option");
            var options = scene.nextScenes.ToList();
            options.RemoveAt(index);
            scene.nextScenes = options.ToArray();
            EditorUtility.SetDirty(scene);
        }

        // Read serialized alternatives, never Check() or the runtime-selected NextScene getter.
        public static IEnumerable<SpecialTransition> GetSpecialTransitions(ScreenSceneScriptableObject scene)
        {
            if (scene == null || scene.nextScenes == null) yield break;
            for (int i = 0; i < scene.nextScenes.Length; i++)
            {
                var dependency = scene.nextScenes[i]?.specialDependent;
                if (dependency == null || !dependency.enable || dependency.special == null) continue;
                foreach (var special in dependency.special)
                {
                    // Title animations always return false and resume the current part asynchronously.
                    if (special == null || special is TitreSpecialScriptableObject) continue;
                    var fields = special is OrBlockSpecialScriptableObject || special is DubleOrSpecialSO
                        ? new[] { "blockTrue", "blockFalse" }
                        : special is AbilitySpecialScriptableObject
                            ? new[] { "beliefScene", "escapeScene", "wastedScene" }
                            : new[] { "nextScene" };
                    using (var serialized = new SerializedObject(special))
                    {
                        foreach (var field in fields)
                        {
                            var target = serialized.FindProperty(field)?.FindPropertyRelative("scene");
                            yield return new SpecialTransition
                            {
                                OptionIndex = i, Asset = special, Field = field,
                                Target = target?.objectReferenceValue as ScreenSceneScriptableObject
                            };
                        }
                    }
                }
            }
        }

        public static List<Issue> Validate(IEnumerable<ScreenSceneScriptableObject> scenes)
        {
            var issues = new List<Issue>();
            foreach (var scene in scenes.Where(s => s != null).Distinct())
            {
                var options = scene.nextScenes ?? Array.Empty<NextScene>();
                for (int i = 0; i < options.Length; i++)
                {
                    var option = options[i];
                    if (option?.scene == null) Add(scene, i, "Missing target scene", scene);
                    else if (option.scene == scene) Add(scene, i, "Scene links directly to itself", scene);
                    var dependency = option?.specialDependent;
                    if (dependency == null || !dependency.enable) continue;
                    if (dependency.special == null || dependency.special.Length == 0)
                        Add(scene, i, "Enabled special transition has no special assets", scene);
                    else foreach (var special in dependency.special)
                        if (special == null) Add(scene, i, "Missing special asset", scene);
                }
                foreach (var special in GetSpecialTransitions(scene))
                {
                    if (special.Target == null) Add(scene, special.OptionIndex, $"{special.Asset.name}.{special.Field}: missing target", special.Asset);
                    else if (special.Target == scene) Add(scene, special.OptionIndex, $"{special.Asset.name}.{special.Field}: scene links directly to itself", special.Asset);
                }
            }
            return issues;

            void Add(ScreenSceneScriptableObject scene, int option, string message, UnityEngine.Object context)
                => issues.Add(new Issue { Scene = scene, OptionIndex = option, Message = message, Context = context });
        }
    }
}
