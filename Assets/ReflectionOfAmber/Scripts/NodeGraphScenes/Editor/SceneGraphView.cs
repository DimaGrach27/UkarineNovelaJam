using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Editor
{
    public class SceneGraphView : GraphView
    {
        public Vector2 NodeSize { get; private set; }= new Vector2(100, 200);
        public SceneGraphView()
        {
            // var styleSheet = Resources.Load<StyleSheet>("SceneGraph_node");
            styleSheets.Add(Resources.Load<StyleSheet>("SceneGraph_node"));
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();
            
            AddElement(GenerateEntrypointNode());
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort != port && startPort.node != port.node)
                {
                    compatiblePorts.Add(port);
                }
            });
            
            return compatiblePorts;
        }

        private Port GeneratePort(
            SceneNode node, 
            Direction portDirection,
            Port.Capacity portCapacity = Port.Capacity.Single)
        {
            return node.InstantiatePort(Orientation.Horizontal, portDirection, portCapacity, typeof(float));
        }

        private SceneNode GenerateEntrypointNode()
        {
            var node = new SceneNode()
            {
                title = "START",
                GUID = Guid.NewGuid().ToString(),
                EntryPoint = true,
                SceneName = "START",
            };
            
            var generatePort = GeneratePort(node, Direction.Output);
            generatePort.portName = "Next";
            node.outputContainer.Add(generatePort);
            
            node.capabilities &= ~Capabilities.Movable;
            node.capabilities &= ~Capabilities.Deletable;
            
            node.RefreshExpandedState();
            node.RefreshPorts();
            
            node.SetPosition(new Rect(100, 200, NodeSize.x, NodeSize.y));
            return node;
        }

        public void CreateNode(string nodeName)
        {
            AddElement(CreateSceneNode(nodeName));
        }

        public SceneNode CreateSceneNode(string sceneName)
        {
            var node = new SceneNode()
            {
                title = sceneName,
                GUID = Guid.NewGuid().ToString(),
                EntryPoint = false,
                SceneName = sceneName
            };
            
            var generateInputPort = GeneratePort(node, Direction.Input, Port.Capacity.Multi);
            generateInputPort.portName = "Input";
            node.inputContainer.Add(generateInputPort);
            
            node.styleSheets.Add(Resources.Load<StyleSheet>("Node"));

            var button = new Button(() => { AddChoicePort(node); });
            button.text = "New Choice";
            
            node.titleContainer.Add(button);
            
            var textField = new TextField(string.Empty);
            textField.RegisterValueChangedCallback(evt =>
            {
                node.SceneName = evt.newValue;
                node.title = evt.newValue;
            });
            textField.SetValueWithoutNotify(node.title);
            node.mainContainer.Add(textField);
            
            node.RefreshExpandedState();
            node.RefreshPorts();

            node.SetPosition(new Rect(0, 0, NodeSize.x, NodeSize.y));
            return node;
        }

        public void AddChoicePort(SceneNode node, string overridenPortName = "")
        {
            var generatePort = GeneratePort(node, Direction.Output);

            var oldLabel = generatePort.contentContainer.Q<Label>("type");
            generatePort.contentContainer.Remove(oldLabel);
            
            var outputPortCount = node.outputContainer.Query("connector").ToList().Count;
            
            var choicePortName = string.IsNullOrEmpty(overridenPortName)
                ? $"Choice{outputPortCount + 1}" 
                : overridenPortName;
            
            var textField = new TextField()
            {
                name = string.Empty,
                value = choicePortName
            };
            textField.RegisterValueChangedCallback(evt => generatePort.portName = evt.newValue);
            generatePort.contentContainer.Add(new Label(" "));
            generatePort.contentContainer.Add(textField);

            var deleteButton = new Button(() => RemovePort(node, generatePort))
            {
                text = "X",
            };
            
            generatePort.contentContainer.Add(deleteButton);
            generatePort.portName = choicePortName;
            
            node.outputContainer.Add(generatePort);
            node.RefreshExpandedState();
            node.RefreshPorts();
        }

        private void RemovePort(SceneNode node, Port generatedPort)
        {
            var targetEdge = edges.ToList()
                .Where(x => x.output.portName == generatedPort.portName && x.output.node == generatedPort.node);

            if (!targetEdge.Any())
            {
                return;
            }

            var edge = targetEdge.First();
            edge.input.Disconnect(edge);
            RemoveElement(targetEdge.First());
            
            node.outputContainer.Remove(generatedPort);
            node.RefreshPorts();
            node.RefreshExpandedState();
        }
    }
}