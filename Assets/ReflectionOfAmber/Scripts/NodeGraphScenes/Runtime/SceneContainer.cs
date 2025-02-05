using System;
using System.Collections.Generic;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Runtime
{
    [Serializable]
    public class SceneContainer : ScriptableObject
    {
        public List<SceneNodeData> SceneData = new ();
        public List<NodeLinkData> NodeLinkData = new ();
    }
}