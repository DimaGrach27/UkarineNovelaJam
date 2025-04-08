using System;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Runtime
{
    [Serializable]
    public class NodeLinkData
    {
        public string BaseNodeGUID;
        public string PortName;
        public string TargetNodeGUID;
    }
}