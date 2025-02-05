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
        private string m_fileName = "New Scene";
        
        [MenuItem("ReflectionOfAmber/Scene Graph")]
        public static void OpenSceneGraphWindow()
        {
            var window = GetWindow<SceneGraph>();
            window.titleContent = new GUIContent("Scene Graph");
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
            GenerateMiniMap();
        }

        private void GenerateMiniMap()
        {
            var miniMap = new MiniMap()
            {
                anchored = true
            };
            
            miniMap.SetPosition(new Rect(10, 30,200, 140));
            m_graphView.Add(miniMap);
        }

        private void ConstructGraphView()
        {
            m_graphView = new SceneGraphView()
            {
                name = "Scene Graph"
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
            
            toolbar.Add(new Button (() => RequestDataOperation(true)){ text = "Save Data" });
            toolbar.Add(new Button (() => RequestDataOperation(false)){ text = "Load Data" });

            var nodeCreateButton = new Button(() => m_graphView.CreateNode("Scene node"));
            nodeCreateButton.text = "Create Node";
            
            toolbar.Add(nodeCreateButton);
            
            rootVisualElement.Add(toolbar);
        }

        private void RequestDataOperation(bool save)
        {
            if (string.IsNullOrWhiteSpace(m_fileName) || string.IsNullOrEmpty(m_fileName))
            {
                EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name", "Ok"); 
                return;
            }

            var graphSaveUtility = GraphSaveUtility.GetInstance(m_graphView);
            if (save)
            {
                graphSaveUtility.SaveGraph(m_fileName);
            }
            else
            {
                graphSaveUtility.LoadGraph(m_fileName);
            }
        }
        
        private void OnDisable()
        {
            rootVisualElement.Remove(m_graphView);
        }
    }
}