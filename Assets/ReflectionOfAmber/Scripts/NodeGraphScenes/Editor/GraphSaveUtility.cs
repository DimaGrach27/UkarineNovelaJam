using System.Collections.Generic;
using System.Linq;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.NodeGraphScenes.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Editor
{
    public class GraphSaveUtility
    {
        private List<Edge> Edges => m_graphView.edges.ToList();
        private List<SceneNode> Nodes => m_graphView.nodes.ToList().Cast<SceneNode>().ToList();

        private List<Group> CommentBlocks =>
            m_graphView.graphElements.ToList().Where(x => x is Group).Cast<Group>().ToList();

        private NarrativeContainer m_narativeContainer;
        private SceneGraphView m_graphView;

        public static GraphSaveUtility GetInstance(SceneGraphView graphView)
        {
            return new GraphSaveUtility
            {
                m_graphView = graphView
            };
        }

        public void SaveGraph(string fileName)
        {
            var narrativeContainer = ScriptableObject.CreateInstance<NarrativeContainer>();
            if (!SaveNodes(fileName, narrativeContainer)) return;
            // SaveExposedProperties(narrativeContainer);
            SaveCommentBlocks(narrativeContainer);

            string path = "Assets/ReflectionOfAmber/Resources/Graphs";
            if (!AssetDatabase.IsValidFolder(path))
                AssetDatabase.CreateFolder("/ReflectionOfAmber/Resources", "Graphs");

            UnityEngine.Object loadedAsset = AssetDatabase.LoadAssetAtPath($"{path}/{fileName}.asset", typeof(NarrativeContainer));

            if (loadedAsset == null || !AssetDatabase.Contains(loadedAsset)) 
			{
                AssetDatabase.CreateAsset(narrativeContainer, $"{path}/{fileName}.asset");
            }
            else 
			{
                NarrativeContainer container = loadedAsset as NarrativeContainer;
                container.NodeLinks = narrativeContainer.NodeLinks;
                container.DialogueNodeData = narrativeContainer.DialogueNodeData;
                container.ExposedProperties = narrativeContainer.ExposedProperties;
                container.CommentBlockData = narrativeContainer.CommentBlockData;
                EditorUtility.SetDirty(container);
            }

            AssetDatabase.SaveAssets();
        }

        private bool SaveNodes(string fileName, NarrativeContainer narrativeContainer)
        {
            if (!Edges.Any()) return false;
            var connectedSockets = Edges.Where(x => x.input.node != null).ToArray();
            for (var i = 0; i < connectedSockets.Count(); i++)
            {
                var outputNode = (connectedSockets[i].output.node as SceneNode);
                var inputNode = (connectedSockets[i].input.node as SceneNode);
                narrativeContainer.NodeLinks.Add(new NodeLinkData
                {
                    BaseNodeGUID = outputNode.GUID,
                    PortName = connectedSockets[i].output.portName,
                    TargetNodeGUID = inputNode.GUID
                });
            }

            foreach (var node in Nodes.Where(node => !node.EntryPoint))
            {
                NextScene[] nextScenes = GetNextScenes(node.GUID);
                node.Scene.nextScenes = nextScenes;
                narrativeContainer.DialogueNodeData.Add(new SceneNodeData()
                {
                    GUID = node.GUID,
                    Key = node.Key,
                    Position = node.GetPosition().position,
                    Scene = node.Scene,
                });
            }

            return true;
        }

        private NextScene[] GetNextScenes(string outputNodeGUID)
        {
            var connectedSockets = Edges.Where(x => x.input.node != null).ToArray();

            List<NextScene> nextScenes = new List<NextScene>();

            foreach (var edge in connectedSockets)
            {
                var outputNode = edge.output.node as SceneNode;
                if (outputNode.GUID == outputNodeGUID)
                {
                    nextScenes.Add(new NextScene()
                    {
                        scene = outputNode.Scene
                    });
                }
            }
            
            return nextScenes.ToArray();
        }

        private void SaveExposedProperties(NarrativeContainer narrativeContainer)
        {
            narrativeContainer.ExposedProperties.Clear();
            narrativeContainer.ExposedProperties.AddRange(m_graphView.ExposedProperties);
        }

        private void SaveCommentBlocks(NarrativeContainer narrativeContainer)
        {
            foreach (var block in CommentBlocks)
            {
                var nodes = block.containedElements.Where(x => x is SceneNode).Cast<SceneNode>().Select(x => x.GUID)
                    .ToList();

                narrativeContainer.CommentBlockData.Add(new CommentBlockData
                {
                    ChildNodes = nodes,
                    Title = block.title,
                    Position = block.GetPosition().position
                });
            }
        }

        public void LoadGraph(string fileName)
        {
            m_narativeContainer = Resources.Load<NarrativeContainer>(fileName);
            if (m_narativeContainer == null)
            {
                EditorUtility.DisplayDialog("File Not Found", "Target Narrative Data does not exist!", "OK");
                return;
            }

            ClearGraph();
            GenerateSceneNodes();
            ConnectSceneNodes();
            AddExposedProperties();
            GenerateCommentBlocks();
        }

        /// <summary>
        /// Set Entry point GUID then Get All Nodes, remove all and their edges. Leave only the entrypoint node. (Remove its edge too)
        /// </summary>
        private void ClearGraph()
        {
            Nodes.Find(x => x.EntryPoint).GUID = m_narativeContainer.NodeLinks[0].BaseNodeGUID;
            foreach (var perNode in Nodes)
            {
                if (perNode.EntryPoint) continue;
                Edges.Where(x => x.input.node == perNode).ToList()
                    .ForEach(edge => m_graphView.RemoveElement(edge));
                m_graphView.RemoveElement(perNode);
            }
        }

        /// <summary>
        /// Create All serialized nodes and assign their guid and dialogue text to them
        /// </summary>
        private void GenerateSceneNodes()
        {
            foreach (var perNode in m_narativeContainer.DialogueNodeData)
            {
                var tempNode = m_graphView.CreateNode(perNode.Key, Vector2.zero);
                tempNode.GUID = perNode.GUID;
                tempNode.Scene = perNode.Scene;
                
                m_graphView.AddElement(tempNode);

                var nodePorts = m_narativeContainer.NodeLinks.Where(x => x.BaseNodeGUID == perNode.GUID).ToList();
                nodePorts.ForEach(x => m_graphView.AddChoicePort(tempNode, x.PortName));
            }
        }

        private void ConnectSceneNodes()
        {
            for (var i = 0; i < Nodes.Count; i++)
            {
                var k = i; //Prevent access to modified closure
                var connections = m_narativeContainer.NodeLinks.Where(x => x.BaseNodeGUID == Nodes[k].GUID).ToList();
                for (var j = 0; j < connections.Count(); j++)
                {
                    var targetNodeGUID = connections[j].TargetNodeGUID;
                    var targetNode = Nodes.First(x => x.GUID == targetNodeGUID);
                    LinkNodesTogether(Nodes[i].outputContainer[j].Q<Port>(), (Port) targetNode.inputContainer[0]);

                    targetNode.SetPosition(new Rect(
                        m_narativeContainer.DialogueNodeData.First(x => x.GUID == targetNodeGUID).Position,
                        m_graphView.DefaultNodeSize));
                }
            }
        }

        private void LinkNodesTogether(Port outputSocket, Port inputSocket)
        {
            var tempEdge = new Edge()
            {
                output = outputSocket,
                input = inputSocket
            };
            tempEdge?.input.Connect(tempEdge);
            tempEdge?.output.Connect(tempEdge);
            m_graphView.Add(tempEdge);
        }

        private void AddExposedProperties()
        {
            m_graphView.ClearBlackBoardAndExposedProperties();
            foreach (var exposedProperty in m_narativeContainer.ExposedProperties)
            {
                m_graphView.AddPropertyToBlackBoard(exposedProperty);
            }
        }

        private void GenerateCommentBlocks()
        {
            foreach (var commentBlock in CommentBlocks)
            {
                m_graphView.RemoveElement(commentBlock);
            }

            foreach (var commentBlockData in m_narativeContainer.CommentBlockData)
            {
               var block = m_graphView.CreateCommentBlock(new Rect(commentBlockData.Position, m_graphView.DefaultCommentBlockSize),
                    commentBlockData);
               block.AddElements(Nodes.Where(x=>commentBlockData.ChildNodes.Contains(x.GUID)));
            }
        }
    }
}