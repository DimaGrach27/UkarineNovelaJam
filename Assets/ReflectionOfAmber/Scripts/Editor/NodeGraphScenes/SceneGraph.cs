using System;
using System.Linq;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart.SpecialSO;
using ReflectionOfAmber.Scripts.NodeGraphScenes.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Editor
{
    public class SceneGraph : EditorWindow
    {
        [SerializeField] private NarrativeContainer source;
        [SerializeField] private NarrativeContainer draft;
        [SerializeField] private bool layoutDirty;
        private SceneGraphView graph;
        private ObjectField layoutField;
        private Label summary;
        private bool refreshQueued;

        [MenuItem("ReflectionOfAmber/Narrative Graph")]
        public static void CreateGraphViewWindow() => GetWindow<SceneGraph>("Narrative Graph");

        public static void FocusScene(ScreenSceneScriptableObject scene)
        {
            if (scene == null) return;
            var window = GetWindow<SceneGraph>("Narrative Graph");
            window.graph.FocusScene(scene);
            window.Focus();
        }

        private void OnEnable()
        {
            titleContent = new GUIContent("Narrative Graph");
            minSize = new Vector2(750, 400);
            rootVisualElement.Clear();
            if (draft == null)
            {
                draft = CreateInstance<NarrativeContainer>();
                draft.hideFlags = HideFlags.HideAndDontSave;
            }
            var toolbar = new Toolbar();
            layoutField = new ObjectField("Layout") { objectType = typeof(NarrativeContainer), allowSceneObjects = false, value = source };
            layoutField.style.width = 280;
            layoutField.RegisterValueChangedCallback(evt =>
            {
                if (ConfirmSwitch()) Load(evt.newValue as NarrativeContainer);
                else layoutField.SetValueWithoutNotify(source);
            });
            toolbar.Add(layoutField);
            toolbar.Add(new ToolbarButton(() => { if (ConfirmSwitch()) Load(source); }) { text = "Reload" });
            toolbar.Add(new ToolbarButton(OpenAll) { text = "All scenes" });
            toolbar.Add(new ToolbarButton(() => Save(false)) { text = "Save" });
            toolbar.Add(new ToolbarButton(() => Save(true)) { text = "Save as…" });
            toolbar.Add(new ToolbarButton(() => ReflectionOfAmber.Scripts.Editor.SceneValidator.Validate()) { text = "Validate" });
            rootVisualElement.Add(toolbar);
            var navigation = new Toolbar();
            var search = new ToolbarSearchField();
            search.style.width = 280;
            search.RegisterCallback<KeyDownEvent>(evt => { if (evt.keyCode == KeyCode.Return) graph.Search(search.value); });
            navigation.Add(search);
            navigation.Add(new ToolbarButton(() => graph.Search(search.value)) { text = "Find" });
            navigation.Add(new ToolbarButton(() => graph.FrameAll()) { text = "Frame all" });
            navigation.Add(new ToolbarButton(() => graph.RefreshFromAssets()) { text = "Refresh links" });
            summary = new Label();
            navigation.Add(summary);
            rootVisualElement.Add(navigation);
            graph = new SceneGraphView(this);
            rootVisualElement.Add(graph);
            graph.LoadLayout(draft);
            rootVisualElement.Add(new Label("Drag an option to another scene. Select Inspector to edit text/conditions. Special links are read-only. START is a layout marker.")
            { style = { whiteSpace = WhiteSpace.Normal } });
            hasUnsavedChanges = layoutDirty;
            saveChangesMessage = "Save changes to the narrative layout? Scene edits remain available in Unity Undo.";
            UpdateSummary();
            Undo.undoRedoPerformed += OnUndoRedo;
            Undo.postprocessModifications += OnModified;
            EditorApplication.projectChanged += QueueRefresh;
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= OnUndoRedo;
            Undo.postprocessModifications -= OnModified;
            EditorApplication.projectChanged -= QueueRefresh;
            EditorApplication.delayCall -= Refresh;
            refreshQueued = false;
        }

        private void OnDestroy()
        {
            if (draft != null) DestroyImmediate(draft);
        }

        private void Load(NarrativeContainer asset)
        {
            source = asset;
            if (draft != null) DestroyImmediate(draft);
            draft = source != null ? Instantiate(source) : CreateInstance<NarrativeContainer>();
            draft.hideFlags = HideFlags.HideAndDontSave;
            layoutField.SetValueWithoutNotify(source);
            graph.LoadLayout(draft);
            layoutDirty = false;
            hasUnsavedChanges = false;
            UpdateSummary();
            graph.schedule.Execute(() => graph.FrameAll());
        }

        private void OpenAll()
        {
            if (!ConfirmSwitch()) return;
            Load(null);
            int index = 0;
            foreach (var scene in SceneGraphData.FindScenes())
            {
                draft.DialogueNodeData.Add(new SceneNodeData
                {
                    GUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(scene)),
                    Key = scene.SceneKey, Scene = scene,
                    Position = new Vector2(index % 8 * 420, index / 8 * 700)
                });
                index++;
            }
            graph.LoadLayout(draft);
            MarkLayoutChanged();
            graph.schedule.Execute(() => graph.FrameAll());
        }

        private bool ConfirmSwitch()
        {
            if (!layoutDirty) return true;
            int action = EditorUtility.DisplayDialogComplex("Unsaved layout", "Save the current layout before switching?", "Save", "Cancel", "Discard layout");
            return action == 2 || action == 0 && Save(false);
        }

        public void MarkLayoutChanged()
        {
            layoutDirty = true;
            hasUnsavedChanges = true;
            UpdateSummary();
        }

        private void UpdateSummary()
        {
            if (summary != null) summary.text = $"  {graph?.nodes.Count() ?? 0} nodes" + (layoutDirty ? " · layout modified" : "");
        }

        public override void SaveChanges()
        {
            if (Save(false)) base.SaveChanges();
        }

        public override void DiscardChanges()
        {
            layoutDirty = false;
            base.DiscardChanges();
        }

        private bool Save(bool saveAs)
        {
            GraphSaveUtility.Capture(graph, draft);
            var target = source;
            if (target == null || saveAs)
            {
                string path = EditorUtility.SaveFilePanelInProject("Save narrative layout", "Narrative", "asset", "Choose layout asset", SceneGraphData.Root);
                if (string.IsNullOrEmpty(path)) return false;
                target = AssetDatabase.LoadAssetAtPath<NarrativeContainer>(path);
                if (target == null)
                {
                    if (AssetDatabase.LoadMainAssetAtPath(path) != null)
                    {
                        ShowNotification(new GUIContent("Choose a layout asset, not a scene"));
                        return false;
                    }
                    target = CreateInstance<NarrativeContainer>();
                    AssetDatabase.CreateAsset(target, path);
                }
            }
            GraphSaveUtility.Save(draft, target);
            foreach (var scene in SceneGraphData.FindScenes())
            {
                AssetDatabase.SaveAssetIfDirty(scene);
                foreach (var special in SceneGraphData.GetSpecialTransitions(scene)) AssetDatabase.SaveAssetIfDirty(special.Asset);
            }
            source = target;
            layoutField.SetValueWithoutNotify(source);
            layoutDirty = false;
            hasUnsavedChanges = false;
            UpdateSummary();
            return true;
        }

        private void OnUndoRedo()
        {
            graph.Rebuild();
            MarkLayoutChanged();
        }

        private UndoPropertyModification[] OnModified(UndoPropertyModification[] modifications)
        {
            if (modifications.Any(m => m.currentValue.target is ScreenSceneScriptableObject || m.currentValue.target is SpecialScriptableObjectBase)) QueueRefresh();
            return modifications;
        }

        private void QueueRefresh()
        {
            if (refreshQueued) return;
            refreshQueued = true;
            EditorApplication.delayCall += Refresh;
        }

        private void Refresh()
        {
            refreshQueued = false;
            if (this != null && graph != null) graph.RefreshFromAssets();
        }
    }
}
