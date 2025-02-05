using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Editor
{
    public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            throw new System.NotImplementedException();
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}