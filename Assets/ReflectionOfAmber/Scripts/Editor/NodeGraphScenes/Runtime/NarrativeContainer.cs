using System;
using System.Collections.Generic;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Runtime
{
    [Serializable]
    public class NarrativeContainer : ScriptableObject
    {
        public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
        public List<SceneNodeData> DialogueNodeData = new List<SceneNodeData>();
        public List<ExposedProperty> ExposedProperties = new List<ExposedProperty>();
        public List<CommentBlockData> CommentBlockData = new List<CommentBlockData>();
    }
}