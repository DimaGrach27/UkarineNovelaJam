using System.Linq;
using ReflectionOfAmber.Scripts.NodeGraphScenes.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Editor
{
    public class SceneGraph : EditorWindow
    {
        private SceneGraphView m_graphView;
        private string m_fileName = "Narrative_test";
        
        private NarrativeContainer m_narrativeContainer;

        [MenuItem("ReflectionOfAmber/Narrative Graph")]
        public static void CreateGraphViewWindow()
        {
            var window = GetWindow<SceneGraph>();
            window.titleContent = new GUIContent("Narrative Graph");
        }

        private void ConstructGraphView()
        {
            m_graphView = new SceneGraphView(this)
            {
                name = "Narrative Graph",
            };
            m_graphView.StretchToParentSize();
            rootVisualElement.Add(m_graphView);
        }

        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();

            var fileNameTextField = new TextField("File Name:");
            fileNameTextField.SetValueWithoutNotify(m_fileName);
            fileNameTextField.MarkDirtyRepaint();
            fileNameTextField.RegisterValueChangedCallback(evt => m_fileName = evt.newValue);
            toolbar.Add(fileNameTextField);

            toolbar.Add(new Button(() => RequestDataOperation(true)) {text = "Save Data"});

            toolbar.Add(new Button(() => RequestDataOperation(false)) {text = "Load Data"});
            // toolbar.Add(new Button(() => _graphView.CreateNewDialogueNode("Dialogue Node")) {text = "New Node",});
            rootVisualElement.Add(toolbar);
        }

        private void RequestDataOperation(bool save)
        {
            if (!string.IsNullOrEmpty(m_fileName))
            {
                var saveUtility = GraphSaveUtility.GetInstance(m_graphView);
                if (save)
                    saveUtility.SaveGraph(m_fileName);
                else
                    saveUtility.LoadGraph($"Graphs/{m_fileName}");
            }
            else
            {
                EditorUtility.DisplayDialog("Invalid File name", "Please Enter a valid filename", "OK");
            }
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
            GenerateMiniMap();
            //GenerateBlackBoard();
        }

        private void GenerateMiniMap()
        {
            var miniMap = new MiniMap {anchored = true};
            // var cords = m_graphView.contentViewContainer.WorldToLocal(new Vector2(this.maxSize.x - 10, 30));
            // miniMap.SetPosition(new Rect(cords.x, cords.y, 200, 140));
            miniMap.SetPosition(new Rect(0, 0, 200, 140));
            m_graphView.Add(miniMap);
        }

        private void GenerateBlackBoard()
        {
            var blackboard = new Blackboard(m_graphView);
            blackboard.Add(new BlackboardSection {title = "Exposed Variables"});
            blackboard.addItemRequested = _blackboard =>
            {
                m_graphView.AddPropertyToBlackBoard(ExposedProperty.CreateInstance(), false);
            };
            blackboard.editTextRequested = (_blackboard, element, newValue) =>
            {
                var oldPropertyName = ((BlackboardField) element).text;
                if (m_graphView.ExposedProperties.Any(x => x.PropertyName == newValue))
                {
                    EditorUtility.DisplayDialog("Error", "This property name already exists, please chose another one.",
                        "OK");
                    return;
                }

                var targetIndex = m_graphView.ExposedProperties.FindIndex(x => x.PropertyName == oldPropertyName);
                m_graphView.ExposedProperties[targetIndex].PropertyName = newValue;
                ((BlackboardField) element).text = newValue;
            };
            blackboard.SetPosition(new Rect(10,30,200,300));
            m_graphView.Add(blackboard);
            m_graphView.Blackboard = blackboard;
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(m_graphView);
        }
    }
}