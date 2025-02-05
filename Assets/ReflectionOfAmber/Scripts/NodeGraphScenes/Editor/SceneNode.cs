using UnityEditor.Experimental.GraphView;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Editor
{
    public class SceneNode : Node
    {
        public string GUID;

        public string SceneName;
        
        public bool EntryPoint = false;
    }
}