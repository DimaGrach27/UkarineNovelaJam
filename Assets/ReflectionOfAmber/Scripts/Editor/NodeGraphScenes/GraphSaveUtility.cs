using System.Linq;
using ReflectionOfAmber.Scripts.NodeGraphScenes.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Editor
{
    // Layout persistence only. Game transitions belong exclusively to scene assets.
    public static class GraphSaveUtility
    {
        public static void Capture(SceneGraphView view, NarrativeContainer layout)
        {
            layout.DialogueNodeData = view.nodes.OfType<SceneNode>().Where(n => !n.EntryPoint)
                .Select(n => new SceneNodeData
                {
                    GUID = n.GUID, Key = n.Key, Scene = n.Scene, ReturnNode = n.ReturnNode,
                    Position = n.GetPosition().position
                }).ToList();
            layout.CommentBlockData = view.graphElements.OfType<Group>().Select(group => new CommentBlockData
            {
                Title = group.title, Position = group.GetPosition().position,
                ChildNodes = group.containedElements.OfType<SceneNode>().Select(n => n.GUID).ToList()
            }).ToList();
            // Visual routing hints preserve return references. Loading validates the hinted scene
            // against actual scene data; these links never change a game transition.
            layout.NodeLinks = view.edges.Where(e => e.output?.node is SceneNode && e.input?.node is SceneNode)
                .Select(e => new NodeLinkData
                {
                    BaseNodeGUID = ((SceneNode)e.output.node).GUID,
                    TargetNodeGUID = ((SceneNode)e.input.node).GUID, PortName = SceneGraphView.GetPortKey(e.output)
                }).ToList();
        }

        public static void Save(NarrativeContainer draft, NarrativeContainer target)
        {
            Undo.RegisterCompleteObjectUndo(target, "Save narrative layout");
            var name = target.name;
            EditorUtility.CopySerialized(draft, target);
            target.name = name;
            target.hideFlags = HideFlags.None;
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssetIfDirty(target);
        }
    }
}
