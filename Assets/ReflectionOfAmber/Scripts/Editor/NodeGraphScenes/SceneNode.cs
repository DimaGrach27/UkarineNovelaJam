using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Editor
{
    public class SceneNode : Node
    {
        public string GUID;

        public string Key;
        public ScreenSceneScriptableObject Scene;
        
        public bool EntryPoint = false;
        public bool ReturnNode = false;

        // Node.GetPosition reads resolvedStyle, which is one layout pass behind SetPosition.
        // Use the inline coordinates so rebuilding/capturing a graph cannot collapse new nodes to (0,0).
        public override Rect GetPosition()
        {
            var rect = base.GetPosition();
            rect.position = new Vector2(style.left.value.value, style.top.value.value);
            return rect;
        }
    }
}
