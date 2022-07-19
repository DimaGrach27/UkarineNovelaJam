using System.Collections.Generic;
using System.Threading.Tasks;
using GameScene.BgScreen;
using GameScene.Characters;
using GameScene.ScreenText;
using GameScene.Services;
using UnityEngine;

namespace GameScene.ScreenPart
{
    public class ScreenPartsService : MonoBehaviour
    {
        private readonly Dictionary<string, ScreenSceneScriptableObject> _screenScenesMap = new();
        
        private int _currentPart;
        private string _currentScene;

        private BgService _bgService;
        private CharacterService _characterService;
        private ScreenTextService _screenTextService;

        private bool _blockClick;

        public void SetServices(
            BgService bgService,
            CharacterService characterService,
            ScreenTextService screenTextService,
            UiClickHandler uiClickHandler
            )
        {
            _bgService = bgService;
            _characterService = characterService;
            _screenTextService = screenTextService;

            ScreenSceneScriptableObject[] list = 
                Resources.LoadAll<ScreenSceneScriptableObject>("Configs/Screens");

            foreach (var screenScene in list)
            {
                _screenScenesMap.Add(screenScene.SceneKey, screenScene);
            }

            uiClickHandler.OnClick += ShowNextPart;
        }
        
        public void Init()
        {
            _currentPart = SaveService.GetPart();
            _currentScene = SaveService.GetScene();
            
            _characterService.HideAllCharacters();
            _screenTextService.HideText();
            
            ShowScene();
        }
        
        public void ShowNextScene(string key)
        {
            _currentScene = key;
            _currentPart = 0;

            SaveService.SaveScene(_currentScene);
            SaveService.SavePart(_currentPart);
            
            ShowScene();
        }

        public void ShowScene()
        {
            ScreenSceneScriptableObject scriptableObject = _screenScenesMap[_currentScene];
                
            _bgService.Show(scriptableObject.Bg);

            ShowPart();
        }
        
        public void ShowNextPart()
        {
            if(_blockClick) return;
            
            _currentPart++;

            if (_currentPart == _screenScenesMap[_currentScene].ScreenParts.Length )
            {
                print($"End scene: {_screenScenesMap[_currentScene].SceneKey}");

                ChooseNextScene();
                return;
            }
            
            ShowPart();
            
            SaveService.SavePart(_currentPart);
        }
        
        public void ShowPart()
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

            ShowNextScene(_screenScenesMap[_currentScene].NextScenes[0].SceneKey);
        }
        
        private void Choose()
        {
            foreach (var scriptableObject in _screenScenesMap[_currentScene].NextScenes)
            {
                print($"Scene: {scriptableObject.SceneKey}");
            }
        }
        
        private async void WaitClick()
        {
            _blockClick = true;
            await Task.Delay((int)(1050 * GlobalConstant.ANIMATION_DISSOLVE_DURATION));
            _blockClick = false;
        }
    }
}