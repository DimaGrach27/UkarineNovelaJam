using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        private bool _inGame;
        
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
            
            _bgService.Show(SaveService.GetCurrentBg());
            
            _inGame = true;
            
            if (_currentScene == "scene_0_0")
            {
                CoroutineHelper.Inst.StartCoroutine(FirstInit());
                return;
            }
            
            AudioSystemService.Inst.StarPlayMusic(MusicType.EMBIENT_SLOW);
            FadeService.FadeService.FadeOut();
            ShowScene();
        }

        IEnumerator FirstInit()
        {
                        
            _currentSceneSo = _screenScenesMap[_currentScene];
            _bgService.Show(_currentSceneSo.ChangeBackGround.bgEnum);
            
            AudioSystemService.Inst.StarPlayMusic(MusicType.NEBO);
            AudioSystemService.Inst.AddQueueClip(MusicType.EMBIENT_SLOW);
            
            float volumeMax = SaveService.GetMusicVolume();
            yield return new WaitForSeconds(0.2f);
            FadeService.FadeService.FadeOut(5.0f);
            AudioSystemService.Inst.AudioSourceMusic.DOFade(volumeMax, 4.0f);
            yield return new WaitForSeconds(5.0f);
            
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
            
            if(_currentSceneSo.StatusSetter.Enable)
            {
                foreach (var statusesValue in _currentSceneSo.StatusSetter.StatusesValues)
                {
                    GameModel.SetStatus(statusesValue.status, statusesValue.value);
                }
            }

            if (_currentSceneSo.CountSetter.Enable)
            {
                int count = GameModel.GetInt(_currentSceneSo.CountSetter.Type);
                count += _currentSceneSo.CountSetter.Count;
                
                GameModel.SetInt(_currentSceneSo.CountSetter.Type, count);
            }
            
            if(_currentSceneSo.ChangeBackGround.enable)
            {
                _bgService.Show(_currentSceneSo.ChangeBackGround.bgEnum);
                SaveService.SetCurrentBg(_currentSceneSo.ChangeBackGround.bgEnum);
            }

            ShowPart();
        }
        
        private void ShowNextPart()
        {
            if(_blockClick || !_inGame) return;
            
            if(_currentPartSo.ActionsTypeEnd != null)
            {
                foreach (var actionType in _currentPartSo.ActionsTypeEnd)
                {
                    _actionScreenService.Action(actionType);
                }
            }
            
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
            if (_currentSceneSo.ScreenParts.Length == 0)
            {
                Debug.Log($"End scene: {_currentSceneSo.SceneKey}");
                ChooseNextScene();
                return;
            }
            
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
                
                AudioSystemService.Inst.PlayShotSound(_currentPartSo.MusicTypeOnStart);

                if(_currentPartSo.StatusSetter.Enable)
                {
                    foreach (var statusesValue in _currentPartSo.StatusSetter.StatusesValues)
                    {
                        GameModel.SetStatus(statusesValue.status, statusesValue.value);
                    }
                }
                
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
                    SceneService.LoadEndGame();
                    _inGame = false;
                    break;
                
                case 1:
                {
                    NextScene nextScene = _currentSceneSo.NextScenes[0];
                    string nexSceneKey = nextScene.Scene.SceneKey;
                
                    if (nextScene.specialDependent.enable)
                    {
                        foreach (var scriptableObject in nextScene.specialDependent.special)
                        {
                            if (scriptableObject.Check())
                            {
                                nexSceneKey = scriptableObject.NextScene.Scene.SceneKey;
                            }
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
            
            _chooseWindowService.SetChooses(PrepareList(false), _currentPartSo.TextShow);
        }

        private void TakePhoto()
        {
            _characterService.HideAllCharacters();
            _screenTextService.HideText();
            
            _chooseWindowService.SetChooses(PrepareList(true), _currentPartSo.TextShow);
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
                    bool result = true;
                    
                    foreach (var statusValue in nextScene.statusDependent.statusesValues)
                    {
                        bool status = GameModel.GetStatus(statusValue.status);
                        result &= status == statusValue.value;
                    }
                    isRemove = !result;
                    
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

            if (chooseScene.findDependent.enable)
            {
                bool result = true;
                    
                foreach (var statusValue in chooseScene.findDependent.statusesValues)
                {
                    bool status = GameModel.GetStatus(statusValue.status);
                    result &= status == statusValue.value;
                }
                
                if(result)
                {
                    Choose();
                    _actionScreenService.Action(ActionType.ALL_ITEM_WAS_FOUND);
                    return;
                }
            }

            if (chooseScene.specialDependent.enable)
            {
                foreach (var scriptableObject in chooseScene.specialDependent.special)
                {
                    if (scriptableObject.Check())
                    {
                        nexSceneKey = scriptableObject.NextScene.Scene.SceneKey;
                    }
                }
            }
            
            ShowNextScene(nexSceneKey);
        }

        private void OnEndTyping()
        {
            _blockClick = false;
        }
    }
}