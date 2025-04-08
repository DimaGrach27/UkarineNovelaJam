using System;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Runtime
{
    
    [Serializable]
    public class SceneNodeData
    {
        public string GUID;

        public string Key;
        public Vector2 Position;

        public ScreenSceneScriptableObject Scene;
        public bool ReturnNode;
    }
}