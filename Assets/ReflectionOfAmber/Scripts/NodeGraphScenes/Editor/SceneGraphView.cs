using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart.SpecialSO;
using ReflectionOfAmber.Scripts.NodeGraphScenes.Editor.Fields;
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
            if (commentBlockData == null)
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
            var field = new BlackboardField { text = localPropertyName, typeText = "string" };
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

        public CameraDependentNode CreateTestNode(Vector2 position)
        {
            var node = new CameraDependentNode();
            node.SetPosition(new Rect(position,
                DefaultNodeSize));

            AddElement(node);
            return node;
        }

        public SceneNode CreateNode(string nodeName, Vector2 position, ScreenSceneScriptableObject obj = null)
        {
            var tempSceneNode = new SceneNode()
            {
                title = nodeName,
                Key = nodeName,
                GUID = Guid.NewGuid().ToString(),
                Scene = obj
            };
            tempSceneNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));
            var inputPort = GetPortInstance(tempSceneNode, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Input";
            tempSceneNode.inputContainer.Add(inputPort);
            tempSceneNode.RefreshExpandedState();
            tempSceneNode.RefreshPorts();
            tempSceneNode.SetPosition(new Rect(position,
                DefaultNodeSize)); //To-Do: implement screen center instantiation positioning

            tempSceneNode.outputContainer.style.alignItems = Align.FlexStart;
            
            var sceneSo = new ObjectField
            {
                objectType = typeof(ScreenSceneScriptableObject),
            };

            sceneSo.RegisterValueChangedCallback(evt =>
            {
                sceneSo.value = evt.newValue;
                ScreenSceneScriptableObject so = (ScreenSceneScriptableObject)sceneSo.value;

                tempSceneNode.Key = so.SceneKey;
                tempSceneNode.title = so.SceneKey;
                tempSceneNode.Scene = so;
            });
            sceneSo.SetValueWithoutNotify(obj);

            tempSceneNode.mainContainer.Add(sceneSo);

            var button = new Button(() => { AddChoicePort(tempSceneNode); })
            {
                text = "Add Next Scene"
            };
            tempSceneNode.titleButtonContainer.Add(button);
            return tempSceneNode;
        }

        public void AddChoicePort(SceneNode nodeCache, string overriddenPortName = "")
        {
            var generatedPort = GetPortInstance(nodeCache, Direction.Output);
            var portLabel = generatedPort.contentContainer.Q<Label>("type");
            generatedPort.contentContainer.Remove(portLabel);

            var outputPortCount = nodeCache.outputContainer.Query("connector").ToList().Count();
            var outputPortName = string.IsNullOrEmpty(overriddenPortName)
                ? $"Option {outputPortCount + 1}"
                : overriddenPortName;

            generatedPort.contentContainer.Add(new Label(outputPortName));
            
            var customContainer = new VisualElement();

            NextScene[] nextScenes = new NextScene[outputPortCount + 1];

            if (nodeCache.Scene != null && nodeCache.Scene.nextScenes != null)
            {
                for (int i = 0; i < nodeCache.Scene.nextScenes.Length; i++)
                {
                    nextScenes[i] = nodeCache.Scene.nextScenes[i];
                }
            }
            
            NextScene nextScene = new NextScene();

            var chooseText = new TextField("Choose: ");
            chooseText.RegisterValueChangedCallback(evt =>
            {
                chooseText.value = evt.newValue;
                nextScene.chooseText = evt.newValue;
            });
            customContainer.Add(chooseText);
            
            StatusDependedField statusDependedField = new (nextScene.statusDependent, customContainer);
            statusDependedField.Build(generatedPort);
            
            CameraDependedField cameraDependedField = new (nextScene.cameraDependent, customContainer);
            cameraDependedField.Build(generatedPort);

            var exclusionToggle = new Toggle("Exclusion dependent: ");
            exclusionToggle.RegisterValueChangedCallback(evt =>
            {
                exclusionToggle.value = evt.newValue;
                nextScene.exclusionDependent.enable = evt.newValue;
            });
            customContainer.Add(exclusionToggle);

            FindDependentField findDependentField = new(nextScene.findDependent, customContainer);
            findDependentField.Build(generatedPort);

            SpecialDependentField specialDependentField = new(nextScene.specialDependent, customContainer);
            specialDependentField.Build(generatedPort);
                
            customContainer.Add(new Label("========"));
            generatedPort.contentContainer.Add(customContainer);

            nextScenes[outputPortCount] = nextScene;

            var deleteButton = new Button(() => RemovePort(nodeCache, generatedPort))
            {
                text = "X"
            };
            
            generatedPort.contentContainer.Add(deleteButton);
            generatedPort.portName = outputPortName;
            nodeCache.outputContainer.Add(generatedPort);
            nodeCache.RefreshExpandedState();
            nodeCache.RefreshPorts();
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

    public class CameraDependentNode : Node
    {
        // public CameraDependent cameraDependent = new CameraDependent();
        // private CameraDependedField cameraDependedField;
        // private StatusDependedField statusDependedField;
        // private FindDependentField findDependentField;
        // private SpecialDependentField specialDependentField;
        //
        // private Spe statusesValues;
        // private List<StatusesValue> statusesFindValues;
        // private List<SpecialScriptableObjectBase> specialArray;
        
        private Port m_outpu1;
        private Port m_outpu2;

        public CameraDependentNode()
        {
            title = "Camera Dependent Node";

            // Додаємо кастомний порт
            // var inputPort = InstantiateCustomPort(Direction.Input, "Input");
            var inputPort =
                InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            inputPort.portName = "Input";

            inputContainer.Add(inputPort);

            m_outpu1 = InstantiateCustomPort(Direction.Output, "Output 1");
            // m_outpu2 = InstantiateCustomPort(Direction.Output, "Output 2");
            outputContainer.Add(m_outpu1);
            // outputContainer.Add(m_outpu2);

            // mainContainer.style.height = 123;
            // Оновлення вузла
            RefreshExpandedState();
            RefreshPorts();
        }

        private Port InstantiateCustomPort(Direction direction, string portName)
        {
            // Створюємо стандартний порт
            var port = InstantiatePort(Orientation.Horizontal, direction, Port.Capacity.Single, typeof(bool));
            port.portName = portName;

            // Головний контейнер для порта та полів
            var portContainer = new VisualElement();
            // portContainer.style.marginBottom = 16;
            // portContainer.style.flexDirection = FlexDirection.Column; // Елементи вертикально
            // portContainer.style.alignItems = Align.Stretch; // Вирівнювання по лівому краю
            // portContainer.style.paddingBottom = 10; // Додаємо відступ знизу
            // portContainer.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.1f); // Для тесту видимості
            // portContainer.style.borderBottomWidth = 1;
            // portContainer.style.borderBottomColor = Color.gray;

            // Додаємо сам порт до контейнера
            // portContainer.Add(port);

            // Додаємо кастомні поля для CameraDependent
            // AddToggleField(portContainer, "Enable", value => cameraDependent.enable = value, cameraDependent.enable);
            // AddToggleField(portContainer, "Is Prep Action", value => cameraDependent.isPrepAction = value,
            // cameraDependent.isPrepAction);
            // AddToggleField(portContainer, "Visible on Photo", value => cameraDependent.visibleOnPhoto = value,
            // cameraDependent.visibleOnPhoto);
            // AddToggleField(portContainer, "Visible out Photo", value => cameraDependent.visibleOutPhoto = value,
            // cameraDependent.visibleOutPhoto);

            // Замінюємо вміст порта на кастомний контейнер
            // port.contentContainer.Clear();

            CameraDependent cameraDependent = new();
            CameraDependedField cameraDependedField = new CameraDependedField(cameraDependent, portContainer);
            cameraDependedField.Build(port);

            StatusDependent statusDependent = new();
            StatusDependedField statusDependedField = new StatusDependedField(statusDependent, portContainer);
            statusDependedField.Build(port);

            FindDependent findDependent = new();
            FindDependentField findDependentField = new FindDependentField(findDependent, portContainer);
            findDependentField.Build(port);

            SpecialDependent specialDependent = new();
            SpecialDependentField specialDependentField = new SpecialDependentField(specialDependent, portContainer);
            specialDependentField.Build(port);
            
            port.contentContainer.Add(portContainer);

            // AddCameraDependent(port);
            
            // var arrayField = CreateArrayField(
            //     "Test Array",
            //     new List<string>(),
            //     () => Debug.Log("Array changed!"), // Викликається після змін
            //     () => "New Item" // Як створювати новий елемент
            // );
            //
            // port.Add(arrayField);

            // Примусово задаємо мінімальну висоту для уникнення накладання
            port.style.minHeight = 40; // Приклад висоти
            port.style.alignItems = Align.Stretch;
            // port.style.paddingBottom = 20;

            return port;
        }

        private void AddCameraDependent(Port cachedPort)
        {
            int blockSize = 60;
            int titleMargin = 4;
            int elementMargin = 12;

            var container = new VisualElement();

            var toggle = new Toggle("Camera dependent: ");
            var togglePrep = new Toggle("isPrepAction");
            var toggleVisible = new Toggle("visibleOnPhoto");
            var toggleOutVisible = new Toggle("visibleOutPhoto");

            toggle.style.marginLeft = titleMargin; // Відступ ліворуч для гарного вигляду
            togglePrep.style.marginLeft = elementMargin; // Відступ ліворуч для гарного вигляду
            toggleVisible.style.marginLeft = elementMargin; // Відступ ліворуч для гарного вигляду
            toggleOutVisible.style.marginLeft = elementMargin; // Відступ ліворуч для гарного вигляду


            toggle.RegisterValueChangedCallback(evt =>
            {
                toggle.value = evt.newValue;
                togglePrep.visible = evt.newValue;
                toggleVisible.visible = evt.newValue;
                toggleOutVisible.visible = evt.newValue;

                if (evt.newValue)
                {
                    container.contentContainer.Add(togglePrep);
                    container.contentContainer.Add(toggleVisible);
                    container.contentContainer.Add(toggleOutVisible);

                    float height = cachedPort.resolvedStyle.height;
                    height += blockSize;

                    cachedPort.style.height = height;
                }
                else
                {
                    container.contentContainer.Remove(togglePrep);
                    container.contentContainer.Remove(toggleVisible);
                    container.contentContainer.Remove(toggleOutVisible);

                    float height = cachedPort.resolvedStyle.height;
                    height -= blockSize;
                    cachedPort.style.height = height;
                }
            });

            togglePrep.RegisterValueChangedCallback(evt => togglePrep.value = evt.newValue);
            toggleVisible.RegisterValueChangedCallback(evt => toggleVisible.value = evt.newValue);
            toggleOutVisible.RegisterValueChangedCallback(evt => toggleOutVisible.value = evt.newValue);

            toggle.SetValueWithoutNotify(false);
            togglePrep.SetValueWithoutNotify(false);
            toggleVisible.SetValueWithoutNotify(false);
            toggleOutVisible.SetValueWithoutNotify(false);

            container.contentContainer.Add(toggle);

            cachedPort.contentContainer.Add(container);
        }

        private void AddToggleField(VisualElement container, string label, Action<bool> onChanged, bool initialValue)
        {
            var toggle = new Toggle(label)
            {
                value = initialValue
            };
            toggle.RegisterValueChangedCallback(evt => onChanged(evt.newValue));
            toggle.style.marginLeft = 5; // Відступ ліворуч для гарного вигляду
            container.Add(toggle);
        }

        private VisualElement CreateArrayField<T>(string label, List<T> array, Action onArrayChanged,
            Func<T> createNewItem)
        {
            // Створюємо Foldout для масиву
            var foldout = new Foldout
            {
                text = label,
                value = true // Відкрити за замовчуванням
            };

            // Контейнер для елементів
            var elementsContainer = new VisualElement();
            elementsContainer.style.flexDirection = FlexDirection.Column;
            foldout.Add(elementsContainer);

            // Кнопка додавання нового елемента
            var addButton = new Button(() =>
            {
                array.Add(createNewItem());
                onArrayChanged?.Invoke();
                RefreshArrayFields(array, elementsContainer, onArrayChanged);
            })
            {
                text = "Add"
            };
            foldout.Add(addButton);

            // Оновлення полів
            RefreshArrayFields(array, elementsContainer, onArrayChanged);

            return foldout;
        }

        private void RefreshArrayFields<T>(List<T> array, VisualElement container, Action onArrayChanged)
        {
            // Очищаємо контейнер
            container.Clear();

            // Додаємо кожен елемент масиву
            for (int i = 0; i < array.Count; i++)
            {
                int index = i;

                // Контейнер для елемента
                var elementContainer = new VisualElement();
                elementContainer.style.flexDirection = FlexDirection.Row;
                elementContainer.style.alignItems = Align.Center;

                // Поле редагування (приклад для string, можна адаптувати під інші типи)
                var textField = new TextField($"Element {i}")
                {
                    value = array[i].ToString()
                };
                textField.RegisterValueChangedCallback(evt =>
                {
                    if (typeof(T) == typeof(string))
                    {
                        array[index] = (T)(object)evt.newValue;
                        onArrayChanged?.Invoke();
                    }
                });
                elementContainer.Add(textField);

                // Кнопка видалення
                var removeButton = new Button(() =>
                {
                    array.RemoveAt(index);
                    onArrayChanged?.Invoke();
                    RefreshArrayFields(array, container, onArrayChanged);
                })
                {
                    text = "X"
                };
                elementContainer.Add(removeButton);

                // Додаємо елемент до контейнера
                container.Add(elementContainer);
            }
        }
    }
}