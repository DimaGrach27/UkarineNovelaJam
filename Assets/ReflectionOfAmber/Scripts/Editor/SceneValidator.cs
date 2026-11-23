using System.Collections.Generic;
using ReflectionOfAmber.Scripts.NodeGraphScenes.Editor;
using UnityEditor;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.Editor
{
    public class SceneValidator : EditorWindow
    {
        private List<SceneGraphData.Issue> issues = new List<SceneGraphData.Issue>();
        private Vector2 scroll;
        private int sceneCount;

        [MenuItem("ReflectionOfAmber/Validate scenes")]
        public static void Validate()
        {
            var window = GetWindow<SceneValidator>("Scene validation");
            window.Scan();
            window.Show();
        }

        private void Scan()
        {
            var scenes = SceneGraphData.FindScenes();
            sceneCount = scenes.Length;
            issues = SceneGraphData.Validate(scenes);
            Repaint();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Validate all scenes in Configs/Graphs")) Scan();
            EditorGUILayout.HelpBox($"{sceneCount} scenes · {issues.Count} issues. Returns through other scenes are allowed.", issues.Count == 0 ? MessageType.Info : MessageType.Warning);
            scroll = EditorGUILayout.BeginScrollView(scroll);
            foreach (var issue in issues)
            {
                if (issue.Scene == null) continue;
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField(issue.ToString(), EditorStyles.wordWrappedLabel);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Select asset"))
                {
                    Selection.activeObject = issue.Context;
                    EditorGUIUtility.PingObject(issue.Context);
                }
                if (GUILayout.Button("Show in graph")) SceneGraph.FocusScene(issue.Scene);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
