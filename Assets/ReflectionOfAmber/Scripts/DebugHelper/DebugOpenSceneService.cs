using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GlobalProject;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ReflectionOfAmber.Scripts.DebugHelper
{
    public class DebugOpenSceneService : MonoBehaviour
    {
        [SerializeField] private TMP_InputField sceneField;
        [SerializeField] private TMP_InputField partField;
        [SerializeField] private Button applyShowBtn;
        [SerializeField] private Toggle openClose;

        private CanvasGroup _canvasGroup;
        private ScreenPartsServiceFacade _screenPartsServiceFacade;
        private SceneService _sceneService;

        [Inject]
        public void Construct(
            ScreenPartsServiceFacade screenPartsServiceFacade,
            SceneService sceneService
            )
        {
            _screenPartsServiceFacade = screenPartsServiceFacade;
            _sceneService = sceneService;
        }
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            
            applyShowBtn.onClick.AddListener(OnClickBtn);
            openClose.onValueChanged.AddListener(OpenClose);

            openClose.isOn = false;
        }

        private void OpenClose(bool isOpen)
        {
            _canvasGroup.alpha = isOpen ? 1.0f : 0.0f;
            _canvasGroup.interactable = isOpen;
            _canvasGroup.blocksRaycasts = isOpen;
        }

        private void OnClickBtn()
        {
            if (GameModel.IsGamePlaying)
            {
                ShowScene();
            }
            else
            {
                SetToSave();
            }
            
            openClose.isOn = false;
        }

        private void SetToSave()
        {
            ScreenSceneScriptableObject scriptableObject = GameModel.GetScene(SceneIndex(sceneField.text));
            if(scriptableObject == null) return;

            int part = 0;
            if (int.TryParse(partField.text, out int result))
            {
                part = result;
            }
            
            SaveService.SaveScene(scriptableObject.SceneKey);
            SaveService.SavePart(part);
            
            _sceneService.LoadGameScene();
        }
        
        private void ShowScene()
        {
            ScreenSceneScriptableObject scriptableObject = GameModel.GetScene(SceneIndex(sceneField.text));
            if(scriptableObject == null) return;

            int part = 0;
            if (int.TryParse(partField.text, out int result))
            {
                part = result;
            }
            
            _screenPartsServiceFacade.PlayNextScene(scriptableObject.SceneKey, part);
        }

        private string SceneIndex(string id)
        {
            string sceneName = "scene_";

            if (id.Contains(sceneName))
            {
                return id;
            }
            
            return sceneName + id;
        }
    }
}