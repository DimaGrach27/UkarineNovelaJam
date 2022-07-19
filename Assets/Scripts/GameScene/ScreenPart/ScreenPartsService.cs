using System.Collections.Generic;
using System.Threading.Tasks;
using GameScene.BgScreen;
using GameScene.Characters;
using GameScene.ChooseWindow;
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

        private bool _blockClick;

        public ScreenPartsService(
            BgService bgService,
            CharacterService characterService,
            ScreenTextService screenTextService,
            UiClickHandler uiClickHandler,
            ChooseWindowService chooseWindowService
            )
        {
            _bgService = bgService;
            _characterService = characterService;
            _screenTextService = screenTextService;
            _chooseWindowService = chooseWindowService;

            ScreenSceneScriptableObject[] list = 
                Resources.LoadAll<ScreenSceneScriptableObject>("Configs/Screens");

            foreach (var screenScene in list)
            {
                _screenScenesMap.Add(screenScene.SceneKey, screenScene);
            }

            chooseWindowService.OnChoose += OnChooseClick;
            uiClickHandler.OnClick += ShowNextPart;
        }
        
        public void Init()
        {
            _currentPart = SaveService.GetPart();
            _currentScene = SaveService.GetScene();
            
            _characterService.HideAllCharacters();
            _screenTextService.HideText();
            _chooseWindowService.ChangeVisible(false);
            
            ShowScene();
        }
        
        private void ShowNextScene(string key)
        {
            _currentScene = key;
            _currentPart = 0;

            SaveService.SaveScene(_currentScene);
            SaveService.SavePart(_currentPart);
            
            ShowScene();
        }

        private void ShowScene()
        {
            ScreenSceneScriptableObject scriptableObject = _screenScenesMap[_currentScene];
                
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
            
            _chooseWindowService.SetChooses(_screenScenesMap[_currentScene].NextScenes);
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