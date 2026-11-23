using System;
using System.Collections.Generic;
using System.Linq;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart.SpecialSO;
using ReflectionOfAmber.Scripts.NodeGraphScenes.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Editor
{
    // Run from the menu or -executeMethod. Checks use disposable objects/assets, never edit story data.
    public static class SceneGraphChecks
    {
        [MenuItem("ReflectionOfAmber/Tests/Scene graph checks")]
        public static void Run()
        {
            var failures = new List<string>();
            int passed = 0;
            Check("Opening preserves metadata and disconnected options", () => WithScenes((a, b) =>
            {
                a.nextScenes = new[] { new NextScene { optionLabel = "Keep label", chooseText = "Keep text" }, new NextScene { scene = b } };
                string before = EditorJsonUtility.ToJson(a);
                var view = new SceneGraphView(null);
                var node = view.CreateNode("A", Vector2.zero, a, false);
                view.AddElement(node);
                Require(node.outputContainer.Query<Port>().ToList().Count == 2, "Missing disconnected option port");
                Require(EditorJsonUtility.ToJson(a) == before, "Opening changed scene data");
            }));
            Check("Direct self links rejected; two-scene cycles allowed", () => WithScenes((a, b) =>
            {
                a.nextScenes = new[] { new NextScene() };
                b.nextScenes = new[] { new NextScene() };
                Require(!SceneGraphData.SetTarget(a, 0, a), "Accepted A→A");
                Require(SceneGraphData.SetTarget(a, 0, b), "Rejected A→B");
                Require(SceneGraphData.SetTarget(b, 0, a), "Rejected B→A");
                Require(SceneGraphData.Validate(new[] { a, b }).Count == 0, "Rejected intentional cycle");
                a.nextScenes[0].scene = a;
                Require(SceneGraphData.Validate(new[] { a, b }).Count == 1, "Existing self link not detected");
            }));
            Check("Disconnect/reconnect and deleting neighbor preserve metadata", () => WithScenes((a, b) =>
            {
                var kept = new NextScene { scene = b, optionLabel = "Choice B", chooseText = "Text B" };
                kept.cameraDependent.enable = true;
                var condition = kept.cameraDependent;
                a.nextScenes = new[] { new NextScene { scene = b }, kept };
                SceneGraphData.SetTarget(a, 1, null);
                Require(a.nextScenes.Length == 2 && a.nextScenes[1] == kept && kept.scene == null, "Disconnect removed or replaced option");
                SceneGraphData.SetTarget(a, 1, b);
                SceneGraphData.RemoveOption(a, 0);
                Require(a.nextScenes.Length == 1 && a.nextScenes[0] == kept, "Wrong option deleted");
                Require(kept.optionLabel == "Choice B" && kept.chooseText == "Text B" && kept.cameraDependent == condition && condition.enable, "Metadata lost");
            }));
            Check("Undo/Redo restore transitions", () => WithScenes((a, b) =>
            {
                a.nextScenes = new[] { new NextScene { chooseText = "Retain" } };
                Undo.IncrementCurrentGroup();
                SceneGraphData.SetTarget(a, 0, b);
                Undo.FlushUndoRecordObjects();
                Undo.IncrementCurrentGroup();
                Undo.PerformUndo();
                Require(a.nextScenes[0].scene == null && a.nextScenes[0].chooseText == "Retain", "Undo failed");
                Undo.PerformRedo();
                Require(a.nextScenes[0].scene == b && a.nextScenes[0].chooseText == "Retain", "Redo failed");
            }));
            Check("Terminal scenes valid; missing options reported", () => WithScenes((a, b) =>
            {
                Require(SceneGraphData.Validate(new[] { a, b }).Count == 0, "Terminal scene invalid");
                a.nextScenes = new NextScene[] { null, new NextScene() };
                Require(SceneGraphData.Validate(new[] { a, b }).Count == 2, "Missing targets not reported");
            }));
            Check("Special alternatives checked without runtime state", () => WithScenes((a, b) =>
            {
                var special = ScriptableObject.CreateInstance<OrBlockSpecialScriptableObject>();
                try
                {
                    using (var so = new SerializedObject(special))
                    {
                        so.FindProperty("blockTrue.scene").objectReferenceValue = a;
                        so.FindProperty("blockFalse.scene").objectReferenceValue = b;
                        so.ApplyModifiedPropertiesWithoutUndo();
                    }
                    a.nextScenes = new[] { new NextScene { scene = b } };
                    a.nextScenes[0].specialDependent.enable = true;
                    a.nextScenes[0].specialDependent.special = new SpecialScriptableObjectBase[] { special };
                    var transitions = SceneGraphData.GetSpecialTransitions(a).ToList();
                    Require(transitions.Count == 2 && transitions[0].Target == a && transitions[1].Target == b, "Wrong serialized branches");
                    Require(SceneGraphData.Validate(new[] { a, b }).Count == 1, "Expected special self link only, not unused base field");
                    a.nextScenes[0].specialDependent.enable = false;
                    Require(SceneGraphData.Validate(new[] { a, b }).Count == 0, "Disabled special reported");
                }
                finally { UnityEngine.Object.DestroyImmediate(special); }
            }));
            Check("Graph uses live scene targets instead of stale saved links", () => WithScenes((a, b) => WithLayout((view, layout) =>
            {
                a.nextScenes = new[] { new NextScene(), new NextScene { scene = b } };
                layout.DialogueNodeData.Add(Data("a", a));
                layout.DialogueNodeData.Add(Data("b", b));
                layout.DialogueNodeData.Add(Data("return-a", a, true));
                layout.NodeLinks.Add(new NodeLinkData { BaseNodeGUID = "a", TargetNodeGUID = "return-a", PortName = "Option 1" });
                string before = EditorJsonUtility.ToJson(a);
                view.LoadLayout(layout);
                var edges = view.edges.ToList();
                Require(edges.Count == 1 && ((SceneNode)edges[0].input.node).Scene == b, "Loaded stale transition");
                Require(EditorJsonUtility.ToJson(a) == before, "Loading mutated scene");
                var source = view.nodes.OfType<SceneNode>().Single(n => n.GUID == "a");
                var output = source.outputContainer.Query<Port>().ToList()[0];
                var compatible = view.GetCompatiblePorts(output, null);
                Require(compatible.All(p => p.direction == Direction.Input && ((SceneNode)p.node).Scene != a), "Allowed wrong direction or duplicate self link");
            })));
            Check("Removing a visual target leaves game transitions intact", () => WithScenes((a, b) => WithLayout((view, layout) =>
            {
                a.nextScenes = new[] { new NextScene { scene = b, chooseText = "Retain" } };
                layout.DialogueNodeData.Add(Data("a", a));
                layout.DialogueNodeData.Add(Data("b", b));
                view.LoadLayout(layout);
                var target = view.nodes.OfType<SceneNode>().Single(n => n.Scene == b);
                view.DeleteElements(new GraphElement[] { target });
                Require(a.nextScenes[0].scene == b, "Deleting visual node cleared game target");
                GraphSaveUtility.Capture(view, layout);
                view.LoadLayout(layout);
                Require(view.nodes.OfType<SceneNode>().Any(n => n.Scene == b && n.ReturnNode), "Missing boundary reference");
            })));
            Check("Disconnecting an edge retains its option", () => WithScenes((a, b) => WithLayout((view, layout) =>
            {
                a.nextScenes = new[] { new NextScene { scene = b, chooseText = "Retain" } };
                layout.DialogueNodeData.Add(Data("a", a));
                layout.DialogueNodeData.Add(Data("b", b));
                view.LoadLayout(layout);
                view.DeleteElements(view.edges.ToList());
                Require(a.nextScenes.Length == 1 && a.nextScenes[0].scene == null && a.nextScenes[0].chooseText == "Retain", "Disconnect changed option metadata");
            })));
            Check("Empty layout saves and reloads; scene metadata never serialized from edges", () => WithScenes((a, b) => WithLayout((view, layout) =>
            {
                var path = "Assets/__SceneGraphCheck_" + Guid.NewGuid().ToString("N") + ".asset";
                var saved = ScriptableObject.CreateInstance<NarrativeContainer>();
                try
                {
                    AssetDatabase.CreateAsset(saved, path);
                    view.LoadLayout(layout);
                    GraphSaveUtility.Capture(view, layout);
                    GraphSaveUtility.Save(layout, saved);
                    Require(saved.DialogueNodeData.Count == 0 && saved.NodeLinks.Count == 0, "Empty layout not saved");
                    layout.DialogueNodeData.Add(Data("a", a));
                    view.LoadLayout(layout);
                    GraphSaveUtility.Capture(view, layout);
                    GraphSaveUtility.Save(layout, saved);
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                    var loaded = AssetDatabase.LoadAssetAtPath<NarrativeContainer>(path);
                    Require(loaded.DialogueNodeData.Count == 1 && loaded.DialogueNodeData[0].GUID == "a", "Terminal node not persisted");
                    Require(a.nextScenes.Length == 0, "Save altered terminal scene");
                }
                finally { Undo.ClearUndo(saved); AssetDatabase.DeleteAsset(path); }
            })));
            Check("Legacy and saved return-node routing survives reload", () => WithScenes((a, b) => WithLayout((view, layout) =>
            {
                a.nextScenes = new[] { new NextScene { scene = b } };
                layout.DialogueNodeData.Add(Data("a", a));
                layout.DialogueNodeData.Add(Data("b", b));
                layout.DialogueNodeData.Add(Data("return-b", b, true));
                layout.NodeLinks.Add(new NodeLinkData { BaseNodeGUID = "a", TargetNodeGUID = "return-b", PortName = "Option 1" });
                view.LoadLayout(layout);
                Require(((SceneNode)view.edges.Single().input.node).GUID == "return-b", "Legacy visual return route lost");
                GraphSaveUtility.Capture(view, layout);
                view.LoadLayout(layout);
                Require(((SceneNode)view.edges.Single().input.node).GUID == "return-b", "Saved visual return route lost");
            })));
            Check("Special boundary references do not overlap", () => WithScenes((a, b) => WithLayout((view, layout) =>
            {
                var c = ScriptableObject.CreateInstance<ScreenSceneScriptableObject>();
                var special = ScriptableObject.CreateInstance<OrBlockSpecialScriptableObject>();
                try
                {
                    using (var so = new SerializedObject(special))
                    {
                        so.FindProperty("blockTrue.scene").objectReferenceValue = b;
                        so.FindProperty("blockFalse.scene").objectReferenceValue = c;
                        so.ApplyModifiedPropertiesWithoutUndo();
                    }
                    a.nextScenes = new[] { new NextScene { scene = b } };
                    a.nextScenes[0].specialDependent.enable = true;
                    a.nextScenes[0].specialDependent.special = new SpecialScriptableObjectBase[] { special };
                    layout.DialogueNodeData.Add(Data("a", a));
                    view.LoadLayout(layout);
                    var references = view.nodes.OfType<SceneNode>().Where(n => n.ReturnNode).ToList();
                    Require(references.Count == 2 && references[0].GetPosition().position != references[1].GetPosition().position,
                        "Boundary alternatives overlap: " + string.Join("; ", references.Select(n => $"{n.Scene.SceneKey}={n.GetPosition()} left={n.style.left} top={n.style.top}")));
                    // Capture one reference, then introduce a second during incremental editing.
                    a.nextScenes[0].specialDependent.enable = false;
                    layout.DialogueNodeData.RemoveAll(n => n.Scene == c);
                    GraphSaveUtility.Capture(view, layout);
                    layout.DialogueNodeData.RemoveAll(n => n.Scene == c);
                    view.LoadLayout(layout);
                    GraphSaveUtility.Capture(view, layout);
                    a.nextScenes[0].specialDependent.enable = true;
                    view.LoadLayout(layout);
                    references = view.nodes.OfType<SceneNode>().Where(n => n.ReturnNode).ToList();
                    Require(references.Count == 2 && references[0].GetPosition().position != references[1].GetPosition().position, "Incrementally added reference overlaps existing reference");
                }
                finally { UnityEngine.Object.DestroyImmediate(c); UnityEngine.Object.DestroyImmediate(special); }
            })));
            Check("Side-effect-only specials do not require a target", () => WithScenes((a, b) =>
            {
                var special = ScriptableObject.CreateInstance<TitreSpecialScriptableObject>();
                try
                {
                    a.nextScenes = new[] { new NextScene { scene = b } };
                    a.nextScenes[0].specialDependent.enable = true;
                    a.nextScenes[0].specialDependent.special = new SpecialScriptableObjectBase[] { special };
                    Require(SceneGraphData.Validate(new[] { a, b }).Count == 0, "Title animation's unused target reported");
                }
                finally { UnityEngine.Object.DestroyImmediate(special); }
            }));
            Check("Existing project layouts load without modifying story assets", () =>
            {
                var scenes = SceneGraphData.FindScenes();
                var snapshots = scenes.ToDictionary(s => s, EditorJsonUtility.ToJson);
                foreach (var guid in AssetDatabase.FindAssets("t:NarrativeContainer", new[] { SceneGraphData.Root }))
                {
                    var original = AssetDatabase.LoadAssetAtPath<NarrativeContainer>(AssetDatabase.GUIDToAssetPath(guid));
                    var copy = UnityEngine.Object.Instantiate(original);
                    try
                    {
                        var view = new SceneGraphView(null);
                        view.LoadLayout(copy);
                        Require(view.nodes.Any(), "No nodes loaded for " + original.name);
                    }
                    finally { UnityEngine.Object.DestroyImmediate(copy); }
                }
                Require(snapshots.All(pair => EditorJsonUtility.ToJson(pair.Key) == pair.Value), "Loading legacy layouts changed story data");
            });
            Check("Positions survive capture before the first UI layout pass", () => WithScenes((a, b) => WithLayout((view, layout) =>
            {
                var data = Data("a", a);
                data.Position = new Vector2(425, 730);
                layout.DialogueNodeData.Add(data);
                view.LoadLayout(layout);
                GraphSaveUtility.Capture(view, layout);
                Require(layout.DialogueNodeData.Single().Position == new Vector2(425, 730), "Capture lost position before layout");
            })));
            if (failures.Count > 0) throw new Exception($"Scene graph checks: {passed} passed, {failures.Count} failed:\n" + string.Join("\n", failures));
            Debug.Log($"Scene graph checks: PASS ({passed} checks)");

            void Check(string name, Action action)
            {
                try { action(); passed++; }
                catch (Exception e) { failures.Add(name + ": " + e); }
            }
        }

        private static SceneNodeData Data(string id, ScreenSceneScriptableObject scene, bool isReturn = false)
            => new SceneNodeData { GUID = id, Key = scene.SceneKey, Scene = scene, ReturnNode = isReturn };

        private static void WithLayout(Action<SceneGraphView, NarrativeContainer> action)
        {
            var layout = ScriptableObject.CreateInstance<NarrativeContainer>();
            try { action(new SceneGraphView(null), layout); }
            finally { Undo.ClearUndo(layout); UnityEngine.Object.DestroyImmediate(layout); }
        }

        private static void WithScenes(Action<ScreenSceneScriptableObject, ScreenSceneScriptableObject> action)
        {
            var a = ScriptableObject.CreateInstance<ScreenSceneScriptableObject>();
            var b = ScriptableObject.CreateInstance<ScreenSceneScriptableObject>();
            a.sceneKey = "A";
            b.sceneKey = "B";
            a.nextScenes = Array.Empty<NextScene>();
            b.nextScenes = Array.Empty<NextScene>();
            try { action(a, b); }
            finally
            {
                Undo.ClearUndo(a); Undo.ClearUndo(b);
                UnityEngine.Object.DestroyImmediate(a); UnityEngine.Object.DestroyImmediate(b);
            }
        }

        private static void Require(bool condition, string message)
        {
            if (!condition) throw new Exception(message);
        }
    }
}
