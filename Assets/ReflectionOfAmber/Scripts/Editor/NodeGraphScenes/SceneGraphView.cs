using System;
using System.Collections.Generic;
using System.Linq;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.NodeGraphScenes.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Editor
{
    public class SceneGraphView : GraphView
    {
        public readonly Vector2 DefaultNodeSize = new Vector2(270, 160);
        public readonly Vector2 DefaultCommentBlockSize = new Vector2(320, 220);
        public NarrativeContainer Layout { get; private set; }
        private readonly SceneGraph owner;
        private bool rebuilding;
        private bool captureQueued;

        private sealed class OptionPort
        {
            public ScreenSceneScriptableObject Scene;
            public int Index;
            public bool ReadOnly;
            public ScreenSceneScriptableObject Target;
            public string Key;
        }

        public SceneGraphView(SceneGraph editorWindow)
        {
            owner = editorWindow;
            style.flexGrow = 1;
            SetupZoom(0.05f, 1.5f);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();
            graphViewChanged = OnGraphChanged;
            groupTitleChanged = (_, __) => QueueCapture();
            elementsAddedToGroup = (_, __) => QueueCapture();
            elementsRemovedFromGroup = (_, __) => QueueCapture();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            var position = contentViewContainer.WorldToLocal(evt.localMousePosition);
            evt.menu.AppendAction("Add scene", _ => CreateNewSceneNode("Scene", position, false));
            evt.menu.AppendAction("Add return reference", _ => CreateNewSceneNode("Return", position, true));
            evt.menu.AppendAction("Add group", _ =>
            {
                RecordLayout();
                CreateCommentBlock(new Rect(position, DefaultCommentBlockSize));
                CaptureLayout();
            });
        }

        public void LoadLayout(NarrativeContainer layout)
        {
            Layout = layout;
            Rebuild();
        }

        public void Rebuild()
        {
            if (Layout == null) return;
            rebuilding = true;
            try
            {
                foreach (var element in graphElements.ToList()) RemoveElement(element);
                var records = Layout.DialogueNodeData ?? new List<SceneNodeData>();
                var primary = new HashSet<ScreenSceneScriptableObject>();
                var ids = new HashSet<string>();
                foreach (var data in records.Where(d => d != null))
                {
                    bool isReturn = data.ReturnNode || (data.Scene != null && !primary.Add(data.Scene));
                    var node = CreateNode(data.Key, data.Position, data.Scene, isReturn);
                    node.GUID = !string.IsNullOrEmpty(data.GUID) && ids.Add(data.GUID) ? data.GUID : Guid.NewGuid().ToString();
                    AddElement(node);
                }

                // References outside this layout stay visible without expanding their whole story.
                var normalNodes = nodes.OfType<SceneNode>().Where(n => !n.ReturnNode).ToList();
                foreach (var node in normalNodes)
                {
                    foreach (var port in node.outputContainer.Query<Port>().ToList())
                    {
                        if (!(port.userData is OptionPort option) || option.Target == null) continue;
                        var hint = Layout.NodeLinks?.FirstOrDefault(link => link != null && link.BaseNodeGUID == node.GUID
                            && (link.PortName == option.Key || !option.ReadOnly && link.PortName == $"Option {option.Index + 1}"));
                        var target = nodes.OfType<SceneNode>().FirstOrDefault(n => n.GUID == hint?.TargetNodeGUID && n.Scene == option.Target)
                            ?? FindTarget(option.Target);
                        if (target == null)
                        {
                            target = CreateNode(option.Target.SceneKey,
                                FindFreeReferencePosition(node), option.Target, true);
                            AddElement(target);
                        }
                        Connect(port, target.inputContainer.Q<Port>(), option.ReadOnly);
                    }
                }

                var entryLink = Layout.NodeLinks?.FirstOrDefault(link => link != null && link.PortName == "Next"
                    && !records.Any(d => d != null && d.GUID == link.BaseNodeGUID));
                var entry = new SceneNode { title = "START (layout marker)", GUID = entryLink?.BaseNodeGUID ?? Guid.NewGuid().ToString(), EntryPoint = true };
                entry.capabilities &= ~Capabilities.Deletable;
                var entryPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                entryPort.portName = "Next";
                entry.outputContainer.Add(entryPort);
                entry.SetPosition(new Rect(-350, 0, 270, 100));
                AddElement(entry);
                var entryTarget = nodes.OfType<SceneNode>().FirstOrDefault(n => n.GUID == entryLink?.TargetNodeGUID);
                if (entryTarget != null) Connect(entryPort, entryTarget.inputContainer.Q<Port>(), false);
                foreach (var groupData in Layout.CommentBlockData ?? new List<CommentBlockData>())
                {
                    if (groupData == null) continue;
                    var group = CreateCommentBlock(new Rect(groupData.Position, DefaultCommentBlockSize), groupData);
                    group.AddElements(nodes.OfType<SceneNode>().Where(n => groupData.ChildNodes != null && groupData.ChildNodes.Contains(n.GUID)));
                }
            }
            finally { rebuilding = false; }
        }

        private SceneNode FindTarget(ScreenSceneScriptableObject scene) => nodes.OfType<SceneNode>()
            .Where(n => n.Scene == scene).OrderBy(n => n.ReturnNode).FirstOrDefault();

        private Vector2 FindFreeReferencePosition(SceneNode source)
        {
            var candidate = source.GetPosition().position + new Vector2(420, 0);
            var occupied = nodes.OfType<SceneNode>().Select(n => new Rect(n.GetPosition().position,
                new Vector2(380, Mathf.Max(240, 180 + n.outputContainer.Query<Port>().ToList().Count * 28)))).ToList();
            while (occupied.Any(rect => rect.Overlaps(new Rect(candidate, new Vector2(380, 240)))))
                candidate.y += 260;
            return candidate;
        }

        public SceneNode CreateNode(string name, Vector2 position, ScreenSceneScriptableObject scene, bool isReturnNode)
        {
            var node = new SceneNode
            {
                GUID = Guid.NewGuid().ToString(), Key = scene != null ? scene.SceneKey : name,
                Scene = scene, ReturnNode = isReturnNode,
                title = (scene != null ? scene.SceneKey : name) + (isReturnNode ? " ↩" : "")
            };
            node.style.minWidth = 270;
            node.SetPosition(new Rect(position, DefaultNodeSize));
            var input = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            input.portName = "Input";
            node.inputContainer.Add(input);
            var field = new ObjectField { objectType = typeof(ScreenSceneScriptableObject), allowSceneObjects = false, value = scene };
            field.style.maxWidth = 280;
            field.RegisterValueChangedCallback(evt =>
            {
                RecordLayout();
                node.Scene = evt.newValue as ScreenSceneScriptableObject;
                node.Key = node.Scene != null ? node.Scene.SceneKey : "Scene";
                CaptureLayout();
                Rebuild();
            });
            node.extensionContainer.Add(field);
            if (scene != null)
            {
                var inspect = new Button(() => { Selection.activeObject = scene; EditorGUIUtility.PingObject(scene); }) { text = "Inspector: text / conditions" };
                node.extensionContainer.Add(inspect);
                if (isReturnNode)
                {
                    node.extensionContainer.Add(new Button(() => Expand(node)) { text = "Expand scene here" });
                }
                else
                {
                    var options = scene.nextScenes ?? Array.Empty<NextScene>();
                    for (int i = 0; i < options.Length; i++)
                    {
                        int index = i;
                        var option = options[i];
                        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                        port.portName = $"{i + 1}. " + (string.IsNullOrEmpty(option?.optionLabel) ? "Next scene" : option.optionLabel);
                        port.tooltip = option?.chooseText ?? "Missing option";
                        port.userData = new OptionPort { Scene = scene, Index = i, Target = option?.scene, Key = $"option:{i}" };
                        port.portColor = option?.scene == scene ? Color.red : option?.scene == null ? new Color(1, 0.65f, 0.2f) : new Color(0.5f, 0.8f, 1);
                        port.Add(new Button(() =>
                        {
                            SceneGraphData.RemoveOption(scene, index);
                            RefreshFromAssets();
                        }) { text = "−", tooltip = "Delete this option, including its text and conditions" });
                        node.outputContainer.Add(port);
                    }
                    foreach (var special in SceneGraphData.GetSpecialTransitions(scene))
                    {
                        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                        port.portName = $"{special.OptionIndex + 1}. {special.Field} (special)";
                        port.portColor = special.Target == scene ? Color.red : new Color(0.8f, 0.6f, 1);
                        port.userData = new OptionPort { Scene = scene, Index = special.OptionIndex, Target = special.Target, ReadOnly = true,
                            Key = $"special:{special.OptionIndex}:{AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(special.Asset))}:{special.Field}" };
                        port.tooltip = special.Asset.name + " — edit shared special asset in Inspector";
                        port.Add(new Button(() => Selection.activeObject = special.Asset) { text = "Inspect" });
                        node.outputContainer.Add(port);
                    }
                    node.extensionContainer.Add(new Button(() =>
                    {
                        SceneGraphData.AddOption(scene);
                        RefreshFromAssets();
                    }) { text = "+ Add option" });
                    if (options.Length == 0) node.extensionContainer.Add(new Label("End of story"));
                }
                if ((scene.nextScenes ?? Array.Empty<NextScene>()).Any(o => o?.scene == scene))
                    node.titleContainer.style.backgroundColor = new Color(0.6f, 0.15f, 0.15f);
            }
            node.RefreshExpandedState();
            node.RefreshPorts();
            return node;
        }

        private void Expand(SceneNode node)
        {
            RecordLayout();
            foreach (var other in nodes.OfType<SceneNode>().Where(n => n.Scene == node.Scene)) other.ReturnNode = true;
            node.ReturnNode = false;
            CaptureLayout();
            Rebuild();
            FocusScene(node.Scene);
        }

        public void CreateNewSceneNode(string name, Vector2 position, bool isReturnNode)
        {
            RecordLayout();
            AddElement(CreateNode(name, position, null, isReturnNode));
            CaptureLayout();
        }

        public Group CreateCommentBlock(Rect rect, CommentBlockData data = null)
        {
            var group = new Group { title = data?.Title ?? "Scene group" };
            group.SetPosition(rect);
            AddElement(group);
            return group;
        }

        public override List<Port> GetCompatiblePorts(Port start, NodeAdapter adapter) => ports.ToList().Where(port =>
            port != start && port.direction != start.direction && port.node != start.node
            && !(start.userData is OptionPort a && a.ReadOnly) && !(port.userData is OptionPort b && b.ReadOnly)
            && !((SceneNode)start.node).EntryPointOrSameScene((SceneNode)port.node)).ToList();

        public static string GetPortKey(Port port) => port.userData is OptionPort option ? option.Key : "Next";

        private GraphViewChange OnGraphChanged(GraphViewChange change)
        {
            if (rebuilding) return change;
            RecordLayout();
            var removedNodes = (change.elementsToRemove ?? new List<GraphElement>()).OfType<SceneNode>().ToHashSet();
            if (change.elementsToRemove != null)
            {
                foreach (var edge in change.elementsToRemove.OfType<Edge>())
                {
                    if (removedNodes.Contains(edge.input?.node as SceneNode) || removedNodes.Contains(edge.output?.node as SceneNode)) continue;
                    if (edge.output?.userData is OptionPort option && !option.ReadOnly)
                        SceneGraphData.SetTarget(option.Scene, option.Index, null);
                }
            }
            if (change.edgesToCreate != null)
            {
                change.edgesToCreate = change.edgesToCreate.Where(edge =>
                {
                    if (!(edge.input?.node is SceneNode target) || !(edge.output?.node is SceneNode source)) return false;
                    if (source.EntryPoint) return !target.EntryPoint;
                    return edge.output.userData is OptionPort option && !option.ReadOnly && target.Scene != null
                        && SceneGraphData.SetTarget(option.Scene, option.Index, target.Scene);
                }).ToList();
            }
            QueueCapture(false);
            return change;
        }

        private static void Connect(Port output, Port input, bool readOnly)
        {
            if (input == null) return;
            var edge = output.ConnectTo(input);
            if (readOnly) edge.capabilities &= ~Capabilities.Deletable;
            output.GetFirstAncestorOfType<GraphView>().AddElement(edge);
        }

        private void RecordLayout()
        {
            if (Layout != null && !rebuilding) Undo.RegisterCompleteObjectUndo(Layout, "Edit narrative layout");
        }

        private void QueueCapture(bool record = true)
        {
            if (rebuilding || captureQueued) return;
            if (record) RecordLayout();
            captureQueued = true;
            schedule.Execute(() =>
            {
                captureQueued = false;
                CaptureLayout();
            });
        }

        public void CaptureLayout()
        {
            if (Layout == null || rebuilding) return;
            GraphSaveUtility.Capture(this, Layout);
            owner?.MarkLayoutChanged();
        }

        public void RefreshFromAssets()
        {
            // Preserve visual positions, then reconstruct links from the current scene assets.
            if (Layout != null) GraphSaveUtility.Capture(this, Layout);
            Rebuild();
        }

        public bool FocusScene(ScreenSceneScriptableObject scene)
        {
            var node = FindTarget(scene);
            if (node == null)
            {
                RecordLayout();
                node = CreateNode(scene.SceneKey, Vector2.zero, scene, false);
                AddElement(node);
                CaptureLayout();
                Rebuild();
                node = FindTarget(scene);
            }
            ClearSelection();
            AddToSelection(node);
            FrameSelection();
            return true;
        }

        public void Search(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            var found = nodes.OfType<SceneNode>().FirstOrDefault(n => n.Scene != null &&
                (n.Scene.SceneKey.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0 || n.Scene.name.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0));
            if (found != null) FocusScene(found.Scene);
            else owner?.ShowNotification(new GUIContent("Scene not found in this layout"));
        }
    }

    internal static class SceneNodeCompatibility
    {
        public static bool EntryPointOrSameScene(this SceneNode a, SceneNode b) => !a.EntryPoint && !b.EntryPoint && a.Scene != null && a.Scene == b.Scene;
    }
}
