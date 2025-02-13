using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using UnityEditor.Experimental.GraphView;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Editor
{
    public class SceneNode : Node
    {
        public string GUID;

        public string Key;
        public ScreenSceneScriptableObject Scene;
        
        public bool EntryPoint = false;
        public bool ReturnNode = false;
    }
}