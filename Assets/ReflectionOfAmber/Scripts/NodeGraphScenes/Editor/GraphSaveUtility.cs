using System.Collections.Generic;
using System.Linq;
using ReflectionOfAmber.Scripts.NodeGraphScenes.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Editor
{
    public class GraphSaveUtility
    {
        private SceneGraphView m_graphView;
        
        private SceneContainer m_cachedSceneContainer;
        
        private List<Edge> Edges => m_graphView.edges.ToList();
        private List<SceneNode> Nodes => m_graphView.nodes.ToList().Cast<SceneNode>().ToList();
        
        public static GraphSaveUtility GetInstance(SceneGraphView targetGraphView)
        {
            return new GraphSaveUtility()
            {
                m_graphView = targetGraphView
            };
        }

        public void SaveGraph(string filename)
        {
            //if there are no edges(no connections) then return
            if (!Edges.Any())
            {
                return;
            }
            
            var sceneContainer = ScriptableObject.CreateInstance<SceneContainer>();

            var connectionPorts = Edges.Where(x => x.input.node != null).ToArray();

            for (int i = 0; i < connectionPorts.Length; i++)
            {
                var outputNode = connectionPorts[i].output.node as SceneNode;
                var inputNode = connectionPorts[i].input.node as SceneNode;
                
                sceneContainer.NodeLinkData.Add(new NodeLinkData()
                {
                    BaseNodeGUID = outputNode.GUID,
                    PortName = connectionPorts[i].output.portName,
                    TargetNodeGUID = inputNode.GUID,
                });
            }

            foreach (var sceneNode in Nodes.Where(node => !node.EntryPoint))
            {
                sceneContainer.SceneData.Add(new SceneNodeData()
                {
                    GUID = sceneNode.GUID,
                    Text = sceneNode.SceneName,
                    Position = sceneNode.GetPosition().position
                });
            }
            
            AssetDatabase.CreateAsset(sceneContainer, $"Assets/ReflectionOfAmber/Resources/{filename}.asset");
            AssetDatabase.SaveAssets();
        }

        public void LoadGraph(string filename)
        {
            m_cachedSceneContainer = Resources.Load<SceneContainer>(filename);

            if (m_cachedSceneContainer == null)
            {
                EditorUtility.DisplayDialog("File Not Found", "Target scene graph file doesn't exists!.", "OK");
                return;
            }

            ClearGraph();
            CreateNodes();
            ConnectNodes();
        }
        
        private void ClearGraph()
        {
            //Set entry point guid back from the save. Discard existing guid.
            Nodes.Find(x => x.EntryPoint).GUID = m_cachedSceneContainer.NodeLinkData[0].BaseNodeGUID;

            foreach (var sceneNode in Nodes)
            {
                if (sceneNode.EntryPoint)
                {
                    continue;
                }
                
                //Remove edges that connected to this node
                Edges.Where(x => x.input.node == sceneNode).ToList()
                    .ForEach(edge => m_graphView.RemoveElement(edge));
                
                //Then remove the node
                m_graphView.RemoveElement(sceneNode);
            }
        }
        
        private void CreateNodes()
        {
            foreach (var sceneNodeData in m_cachedSceneContainer.SceneData)
            {
                var tempNode = m_graphView.CreateSceneNode(sceneNodeData.Text);
                tempNode.GUID = sceneNodeData.GUID;
                m_graphView.AddElement(tempNode);

                var nodePorts = m_cachedSceneContainer.NodeLinkData.
                    Where(x => x.BaseNodeGUID == sceneNodeData.GUID).ToList();
                
                nodePorts.ForEach(x => m_graphView.AddChoicePort(tempNode, x.PortName));
            }
        }
        
        private void ConnectNodes()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                var connections = m_cachedSceneContainer.NodeLinkData.
                    Where(x => x.BaseNodeGUID == Nodes[i].GUID).ToList();
                for (int j = 0; j < connections.Count; j++)
                {
                    var targetNodeGuid = connections[j].TargetNodeGUID;
                    var targetNode = Nodes.First(x => x.GUID == targetNodeGuid);
                    LinkNodes(Nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);
                    
                    targetNode.SetPosition(new Rect(
                        m_cachedSceneContainer.SceneData.First(x => x.GUID == targetNodeGuid).Position, 
                        m_graphView.NodeSize));
                }
            }
        }

        private void LinkNodes(Port output, Port input)
        {
            var tempEdge = new Edge()
            {
                output = output,
                input = input
            };
            
            tempEdge?.input.Connect(tempEdge);
            tempEdge?.output.Connect(tempEdge);
            
            m_graphView.AddElement(tempEdge);
        }
    }
}