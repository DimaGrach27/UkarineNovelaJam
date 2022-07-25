using System.Collections;
using System.Collections.Generic;
using GameScene.BgScreen;
using GameScene.Characters;
using GameScene.ChooseWindow;
using GameScene.ChooseWindow.CameraAction;
using GameScene.ScreenPart.ActionScreens;
using GameScene.ScreenText;
using GameScene.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameScene.ScreenPart
{
    public class ScreenPartsService 
    {
        private readonly Dictionary<string, ScreenSceneScriptableObject> _screenScenesMap = new();
        
        private int _currentPart;
        private string _currentScene;

        private readonly BgService _bgService;
        private readonly CharacterService _characterService;
        private readonly ScreenTextService _screenTextService;
        private readonly ChooseWindowService _chooseWindowService;
        private readonly CameraActionService _cameraActionService;
        private readonly ActionScreenService _actionScreenService;

        private bool _blockClick;
        private bool _inGame;

        private Coroutine _loadCoroutine;
        private ScreenSceneScriptableObject _currentSceneSo;
        private ScreenPart _currentPartSo;

        public ScreenPartsService(
            BgService bgService,
            CharacterService characterService,
            ScreenTextService screenTextService,
            UiClickHandler uiClickHandler,
            ChooseWindowService chooseWindowService,
            CameraActionService cameraActionService,
            ActionScreenService actionScreenService
            )
        {
            _bgService = bgService;
            _characterService = characterService;
            _screenTextService = screenTextService;
            _chooseWindowService = chooseWindowService;
            _cameraActionService = cameraActionService;
            _actionScreenService = actionScreenService;

            ScreenSceneScriptableObject[] list = 
                Resources.LoadAll<ScreenSceneScriptableObject>("Configs/Screens");

            foreach (var screenScene in list)
            {
                _screenScenesMap.Add(screenScene.SceneKey, screenScene);
            }

            screenTextService.OnEndTyping += OnEndTyping;
            chooseWindowService.OnChoose += OnChooseClick;
            cameraActionService.OnTakePhoto += TakePhoto;
            uiClickHandler.OnClick += ShowNextPart;
        }
        
        public void Init()
        {
            _currentPart = SaveService.GetPart;
            _currentScene = SaveService.GetScene;
            
            _characterService.HideAllCharactersInstant();
            _screenTextService.HideText();
            _chooseWindowService.ChangeVisible(false);
            _cameraActionService.ChangeVisible(false);
            
            _inGame = true;
            
            ShowScene();
        }
        
        private void ShowNextScene(string key)
        {
            _currentScene = key;
            _currentPart = 0;

            SaveService.SaveScene(_currentScene);
            SaveService.SavePart(_currentPart);
            
            _characterService.HideAllCharacters();
            _screenTextService.HideText();
            _chooseWindowService.ChangeVisible(false);
            _cameraActionService.ChangeVisible(false);
            
            ShowScene();
        }

        private void ShowScene()
        {
            _currentSceneSo = _screenScenesMap[_currentScene];
            
            if(_currentSceneSo.ActionsType != null)
            {
                foreach (var actionType in _currentSceneSo.ActionsType)
                {
                    _actionScreenService.Action(actionType);
                }
            }
            
            if (_currentSceneSo.StatusSetter.Enable)
            {
                GameModel.SetStatus(
                    _currentSceneSo.StatusSetter.Status,
                    _currentSceneSo.StatusSetter.StatusFlag);
            }

            if (_currentSceneSo.CountSetter.Enable)
            {
                int count = GameModel.GetInt(_currentSceneSo.CountSetter.Type);
                count += _currentSceneSo.CountSetter.Count;
                
                GameModel.SetInt(_currentSceneSo.CountSetter.Type, count);
            }
            
            _bgService.Show(_currentSceneSo.Bg);

            ShowPart();
        }
        
        private void ShowNextPart()
        {
            if(_blockClick || !_inGame) return;
            
            
            Debug.Log($"SHOW next: {_currentPart}");
            _currentPart++;

            if (_currentPart >= _currentSceneSo.ScreenParts.Length )
            {
                Debug.Log($"End scene: {_currentSceneSo.SceneKey}");

                ChooseNextScene();
                return;
            }
            
            ShowPart();
            
            SaveService.SavePart(_currentPart);
        }
        
        private void ShowPart()
        {
            if (_currentPart < _currentSceneSo.ScreenParts.Length)
            {
                _currentPartSo = _currentSceneSo.ScreenParts[_currentPart];
                
                if(_currentPartSo.ActionsType != null)
                {
                    foreach (var actionType in _currentPartSo.ActionsType)
                    {
                        _actionScreenService.Action(actionType);
                    }
                }

                if(_currentPartSo.StatusSetter.Enable)
                    GameModel.SetStatus(
                        _currentPartSo.StatusSetter.Status, 
                        _currentPartSo.StatusSetter.StatusFlag);
                
                _characterService.ShowCharacter(_currentPartSo.Position, _currentPartSo.Image);
                _screenTextService.SetText(_currentPartSo.CharacterName, _currentPartSo.TextShow);
                
                _blockClick = true;
            }
        }

        private void ChooseNextScene()
        {
            switch (_currentSceneSo.NextScenes.Length)
            {
                case 0:
                    LoadEndGame();
                    _inGame = false;
                    break;
                
                case 1:
                {
                    NextScene nextScene = _currentSceneSo.NextScenes[0];
                    string nexSceneKey = nextScene.Scene.SceneKey;
                
                    if (nextScene.specialDependent.enable)
                    {
                        if (nextScene.specialDependent.special.Check())
                        {
                            nexSceneKey = nextScene.specialDependent.special.NextScene().Scene.SceneKey;
                        }
                    }
                
                    ShowNextScene(nexSceneKey);
                    break;
                }
                
                case > 1:
                    Choose();
                    break;
            }
        }

        private void Choose()
        {
            _characterService.HideAllCharacters();
            _screenTextService.HideText();
            _cameraActionService.ChangeVisible(_currentSceneSo.IsActiveCamera);
            
            _chooseWindowService.SetChooses(PrepareList(false));
        }

        private void TakePhoto()
        {
            _characterService.HideAllCharacters();
            _screenTextService.HideText();
            
            _chooseWindowService.SetChooses(PrepareList(true));
        }

        private NextScene[] PrepareList(bool isCamera)
        {
            ChoosesList choosesList = 
                SaveService.GetListFromJson(_currentSceneSo.SceneKey, out bool isFirstInit);
            
            int count = 0;
            
            if (isFirstInit)
            {
                int countChooses = _currentSceneSo.NextScenes.Length;
                
                choosesList.blockKey = _currentSceneSo.SceneKey;
                choosesList.chooseKeys = new string[countChooses];
                choosesList.chooseStatus = new bool[countChooses];
                
                for (int i = 0; i < countChooses; i++)
                {
                    choosesList.chooseKeys[i] = _currentSceneSo.NextScenes[i].Scene.SceneKey;
                    choosesList.chooseStatus[i] = false;
                }
                
                SaveService.SetChoosesList(_currentSceneSo.SceneKey, choosesList);
            }
            
            List<NextScene> nextScenes = new();

            foreach (var nextScene in _currentSceneSo.NextScenes)
            {
                nextScenes.Add(nextScene);

                bool isRemove = false;
                
                if (nextScene.statusDependent.enable)
                {
                    if (nextScene.statusDependent.value !=
                        GameModel.GetStatus(nextScene.statusDependent.status))
                    {
                        isRemove = true;
                    }
                }
                
                if (nextScene.cameraDependent.enable)
                {
                    if(isCamera)
                        isRemove |= !nextScene.cameraDependent.visibleOnPhoto;
                    else
                        isRemove |= !nextScene.cameraDependent.visibleOutPhoto;
                }
                
                if (nextScene.exclusionDependent.enable)
                {
                    isRemove |= choosesList.chooseStatus[count];
                }

                if (isRemove) nextScenes.Remove(nextScene);

                count++;
            }

            return nextScenes.ToArray();
        }

        private void OnChooseClick(NextScene chooseScene)
        {
            string nexSceneKey = chooseScene.Scene.SceneKey;
            
            if (chooseScene.exclusionDependent.enable)
            {
                SaveService.SetChoose(_currentSceneSo.SceneKey, chooseScene.Scene.SceneKey);
            }

            if (chooseScene.findDependent.enable &&
                GameModel.GetStatus(chooseScene.findDependent.status) == 
                chooseScene.findDependent.value)
            {
                Choose();
                _actionScreenService.Action(ActionType.ALL_ITEM_WAS_FOUND);
                return;
            }

            if (chooseScene.specialDependent.enable)
            {
                if (chooseScene.specialDependent.special.Check())
                {
                    nexSceneKey = chooseScene.specialDependent.special.NextScene().Scene.SceneKey;
                }
            }
            
            ShowNextScene(nexSceneKey);
        }

        private void OnEndTyping()
        {
            _blockClick = false;
        }

        private void LoadEndGame()
        {
            if(_loadCoroutine != null) return;

            _loadCoroutine = CoroutineHelper.Inst.StartCoroutine(LoadRoutine());
        }

        private IEnumerator LoadRoutine()
        {
            FadeService.FadeService.FadeIn();
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene("EndScene");
        }
    }
}