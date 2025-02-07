using System;
using System.Collections.Generic;
using System.Linq;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.NodeGraphScenes.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Editor
{
    public class SceneGraphView : GraphView
    {
        public readonly Vector2 DefaultNodeSize = new Vector2(200, 150);
        public readonly Vector2 DefaultCommentBlockSize = new Vector2(300, 200);
        public SceneNode EntryPointNode;
        public Blackboard Blackboard = new Blackboard();
        public List<ExposedProperty> ExposedProperties { get; private set; } = new List<ExposedProperty>();
        private NodeSearchWindow _searchWindow;
        
        //Status setter elements
        private Label m_status;
        private EnumField m_statusEnums;
        private Toggle m_statusValues;

        public SceneGraphView(SceneGraph editorWindow)
        {
            styleSheets.Add(Resources.Load<StyleSheet>("Grid"));
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new FreehandSelector());

            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            AddElement(GetEntryPointNodeInstance());

            AddSearchWindow(editorWindow);
        }
        
        private void AddSearchWindow(SceneGraph editorWindow)
        {
            _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
            _searchWindow.Configure(editorWindow, this);
            nodeCreationRequest = context =>
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }
        
        public void ClearBlackBoardAndExposedProperties()
        {
            ExposedProperties.Clear();
            Blackboard.Clear();
        }

        public Group CreateCommentBlock(Rect rect, CommentBlockData commentBlockData = null)
        {
            if(commentBlockData==null)
                commentBlockData = new CommentBlockData();
            var group = new Group
            {
                autoUpdateGeometry = true,
                title = commentBlockData.Title
            };
            AddElement(group);
            group.SetPosition(rect);
            return group;
        }

        public void AddPropertyToBlackBoard(ExposedProperty property, bool loadMode = false)
        {
            var localPropertyName = property.PropertyName;
            var localPropertyValue = property.PropertyValue;
            if (!loadMode)
            {
                while (ExposedProperties.Any(x => x.PropertyName == localPropertyName))
                    localPropertyName = $"{localPropertyName}(1)";
            }

            var item = ExposedProperty.CreateInstance();
            item.PropertyName = localPropertyName;
            item.PropertyValue = localPropertyValue;
            ExposedProperties.Add(item);

            var container = new VisualElement();
            var field = new BlackboardField {text = localPropertyName, typeText = "string"};
            container.Add(field);

            var propertyValueTextField = new TextField("Value:")
            {
                value = localPropertyValue
            };
            propertyValueTextField.RegisterValueChangedCallback(evt =>
            {
                var index = ExposedProperties.FindIndex(x => x.PropertyName == item.PropertyName);
                ExposedProperties[index].PropertyValue = evt.newValue;
            });
            var sa = new BlackboardRow(field, propertyValueTextField);
            container.Add(sa);
            Blackboard.Add(container);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            var startPortView = startPort;

            ports.ForEach((port) =>
            {
                var portView = port;
                if (startPortView != portView && startPortView.node != portView.node)
                    compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

        public void CreateNewSceneNode(string nodeName, Vector2 position)
        {
            AddElement(CreateNode(nodeName, position));
        }

        public SceneNode CreateNode(string nodeName, Vector2 position)
        {
            var tempSceneNode = new SceneNode()
            {
                title = nodeName,
                Key = nodeName,
                GUID = Guid.NewGuid().ToString()
            };
            tempSceneNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));
            var inputPort = GetPortInstance(tempSceneNode, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Input";
            tempSceneNode.inputContainer.Add(inputPort);
            tempSceneNode.RefreshExpandedState();
            tempSceneNode.RefreshPorts();
            tempSceneNode.SetPosition(new Rect(position,
                DefaultNodeSize)); //To-Do: implement screen center instantiation positioning

            // var textField = new TextField("");
            // textField.RegisterValueChangedCallback(evt =>
            // {
            //     tempSceneNode.Key = evt.newValue;
            //     tempSceneNode.title = evt.newValue;
            // });
            // textField.SetValueWithoutNotify(tempSceneNode.title);
            // tempSceneNode.mainContainer.Add(textField);
            
            // var toggle = new Toggle("IsActiveCamera");
            // toggle.RegisterValueChangedCallback(evt =>
            // {
            //     tempSceneNode.IsCameraActive = evt.newValue;
            // });
            // toggle.SetValueWithoutNotify(false);
            // tempSceneNode.mainContainer.Add(toggle);
            
            //AddStatusSetter(tempSceneNode);

            var sceneSo = new ObjectField
            {
                objectType = typeof(ScreenSceneScriptableObject)
            };
            
            sceneSo.RegisterValueChangedCallback(evt =>
            {
                sceneSo.value = evt.newValue;
                ScreenSceneScriptableObject so = (ScreenSceneScriptableObject)sceneSo.value;

                tempSceneNode.Key = so.SceneKey;
                tempSceneNode.title = so.SceneKey;
                tempSceneNode.Scene = so;
            });
            
            tempSceneNode.mainContainer.Add(sceneSo);

            var button = new Button(() => { AddChoicePort(tempSceneNode); })
            {
                text = "Add Choice"
            };
            tempSceneNode.titleButtonContainer.Add(button);
            return tempSceneNode;
        }

        // private void AddStatusSetter(SceneNode node)
        // {
            // StatusSetter statusSetter = new StatusSetter()
            // {
            //     enable = false,
            // };
            // node.StatusSetter = statusSetter;
            //
            // var obj = new StatusSetterUIElement(node);
            // node.mainContainer.Add(obj);
            
            // var toggle = new Toggle("Status setter Enabled");//
            // toggle.RegisterValueChangedCallback(evt =>
            // {
                // node.StatusSetter.enable = evt.newValue;

                // if (node.StatusSetter.enable)
                // {
                    // m_status = new Label("Status setter");
                    // var listView = new ListView();
                    // listView.itemsSource = new List<StatusesValue>();
                    // listView.itemsSource.Add(new StatusesValue()
                    // {
                    //     value = evt.newValue,
                    //     status = StatusEnum.NONE
                    // });
                    
                    // node.mainContainer.Add(m_status);
                    // node.mainContainer.Add(m_statusEnums);
                    // node.mainContainer.Add(m_statusValues);
                    // node.mainContainer.Add(listView);
                // }
                // else
                // {
                    // node.mainContainer.Remove(m_status);
                    // node.mainContainer.Remove(m_statusEnums);
                    // node.mainContainer.Remove(m_statusValues);
                // }
            // });
            // toggle.SetValueWithoutNotify(false);
            // node.mainContainer.Add(toggle);
        // }


        public void AddChoicePort(SceneNode nodeCache, string overriddenPortName = "")
        {
            var generatedPort = GetPortInstance(nodeCache, Direction.Output);
            var portLabel = generatedPort.contentContainer.Q<Label>("type");
            generatedPort.contentContainer.Remove(portLabel);

            var outputPortCount = nodeCache.outputContainer.Query("connector").ToList().Count();
            var outputPortName = string.IsNullOrEmpty(overriddenPortName)
                ? $"Option {outputPortCount + 1}"
                : overriddenPortName;


            var textField = new TextField()
            {
                name = string.Empty,
                value = outputPortName
            };
            textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
            generatedPort.contentContainer.Add(new Label("  "));
            generatedPort.contentContainer.Add(textField);
            var deleteButton = new Button(() => RemovePort(nodeCache, generatedPort))
            {
                text = "X"
            };
            generatedPort.contentContainer.Add(deleteButton);
            generatedPort.portName = outputPortName;
            nodeCache.outputContainer.Add(generatedPort);
            nodeCache.RefreshPorts();
            nodeCache.RefreshExpandedState();
        }

        private void RemovePort(Node node, Port socket)
        {
            var targetEdge = edges.ToList()
                .Where(x => x.output.portName == socket.portName && x.output.node == socket.node);
            if (targetEdge.Any())
            {
                var edge = targetEdge.First();
                edge.input.Disconnect(edge);
                RemoveElement(targetEdge.First());
            }

            node.outputContainer.Remove(socket);
            node.RefreshPorts();
            node.RefreshExpandedState();
        }

        private Port GetPortInstance(SceneNode node, Direction nodeDirection,
            Port.Capacity capacity = Port.Capacity.Single)
        {
            return node.InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
        }

        private SceneNode GetEntryPointNodeInstance()
        {
            var nodeCache = new SceneNode()
            {
                title = "START",
                GUID = Guid.NewGuid().ToString(),
                Key = "ENTRYPOINT",
                EntryPoint = true
            };

            var generatedPort = GetPortInstance(nodeCache, Direction.Output);
            generatedPort.portName = "Next";
            nodeCache.outputContainer.Add(generatedPort);

            nodeCache.capabilities &= ~Capabilities.Movable;
            nodeCache.capabilities &= ~Capabilities.Deletable;

            nodeCache.RefreshExpandedState();
            nodeCache.RefreshPorts();
            nodeCache.SetPosition(new Rect(100, 200, 100, 150));
            return nodeCache;
        }
    }
}