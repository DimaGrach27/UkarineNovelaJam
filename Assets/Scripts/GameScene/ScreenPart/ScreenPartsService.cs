using System.Collections.Generic;
using System.Threading.Tasks;
using GameScene.BgScreen;
using GameScene.Characters;
using GameScene.ChooseWindow;
using GameScene.ChooseWindow.CameraAction;
using GameScene.ScreenPart.ActionScreens;
using GameScene.ScreenText;
using GameScene.Services;
using UnityEngine;

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

            chooseWindowService.OnChoose += OnChooseClick;
            cameraActionService.OnTakePhoto += TakePhoto;
            uiClickHandler.OnClick += ShowNextPart;
        }
        
        public void Init()
        {
            _currentPart = SaveService.GetPart;
            _currentScene = SaveService.GetScene;
            
            _characterService.HideAllCharacters();
            _screenTextService.HideText();
            _chooseWindowService.ChangeVisible(false);
            _cameraActionService.ChangeVisible(false);
            
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
            ScreenSceneScriptableObject scriptableObject = _screenScenesMap[_currentScene];
                
            _actionScreenService.Action(scriptableObject.ActionType);
            
            _bgService.Show(scriptableObject.Bg);

            ShowPart();
        }
        
        private void ShowNextPart()
        {
            if(_blockClick) return;
            
            _currentPart++;

            if (_currentPart == _screenScenesMap[_currentScene].ScreenParts.Length )
            {
                Debug.Log($"End scene: {_screenScenesMap[_currentScene].SceneKey}");

                ChooseNextScene();
                return;
            }
            
            ShowPart();
            
            SaveService.SavePart(_currentPart);
        }
        
        private void ShowPart()
        {
            if (_currentPart < _screenScenesMap[_currentScene].ScreenParts.Length)
            {
                ScreenPart screenPart = _screenScenesMap[_currentScene].ScreenParts[_currentPart];
                
                _actionScreenService.Action(screenPart.ActionType);
                
                _characterService.ShowCharacter(screenPart.Position, screenPart.Image);
                _screenTextService.SetText(screenPart.CharacterName, screenPart.TextShow);

                WaitClick();
            }
        }

        private void ChooseNextScene()
        {
            if (_screenScenesMap[_currentScene].NextScenes.Length > 1)
            {
                Choose();
                return;
            }

            ShowNextScene(_screenScenesMap[_currentScene].NextScenes[0].Scene.SceneKey);
        }
        
        private void Choose()
        {
            _characterService.HideAllCharacters();
            _screenTextService.HideText();
            
            _cameraActionService.ChangeVisible(true);
            
            _chooseWindowService.SetChooses(
                PrepareList(
                    _screenScenesMap[_currentScene].NextScenes, 
                    false, 
                    true));
        }

        private void TakePhoto()
        {
            _characterService.HideAllCharacters();
            _screenTextService.HideText();
            
            _chooseWindowService.SetChooses(
                PrepareList(
                    _screenScenesMap[_currentScene].NextScenes, 
                    true, 
                    false));
        }

        private NextScene[] PrepareList(NextScene[] list, bool isShowOnCameraAction, bool isReadyToShow)
        {
            List<NextScene> resultList = new List<NextScene>();

            foreach (var nextScene in list)
            {
                if (nextScene.IsReadyToShow == isReadyToShow)
                {
                    if(!resultList.Contains(nextScene))
                        resultList.Add(nextScene);
                }
                
                if (nextScene.IsShowOnCameraAction == isShowOnCameraAction)
                {
                    if(!resultList.Contains(nextScene))
                        resultList.Add(nextScene);
                }
            }
            
            return resultList.ToArray();
        }

        private void OnChooseClick(NextScene chooseScene)
        {
            ShowNextScene(chooseScene.Scene.SceneKey);
        }
        
        private async void WaitClick()
        {
            _blockClick = true;
            await Task.Delay((int)(1050 * GlobalConstant.ANIMATION_DISSOLVE_DURATION));
            _blockClick = false;
        }
    }
}