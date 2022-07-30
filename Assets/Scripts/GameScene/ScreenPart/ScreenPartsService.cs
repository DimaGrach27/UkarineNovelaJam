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

namespace GameScene.ScreenPart
{
    public class ScreenPartsService 
    {
        private int _currentPart;
        private string _currentScene;

        private BgService _bgService;
        private CharacterService _characterService;
        private ScreenTextService _screenTextService;
        private ChooseWindowService _chooseWindowService;
        private CameraActionService _cameraActionService;
        private ActionScreenService _actionScreenService;

        private bool _blockClick;
        private bool _inGame;
        
        private ScreenSceneScriptableObject _currentSceneSo;
        private ScreenPart _currentPartSo;

        private Coroutine _changeBgRoutine;

        public void InitServices(BgService bgService,
            CharacterService characterService,
            ScreenTextService screenTextService,
            UiClickHandler uiClickHandler,
            ChooseWindowService chooseWindowService,
            CameraActionService cameraActionService,
            ActionScreenService actionScreenService)
        {
            _bgService = bgService;
            _characterService = characterService;
            _screenTextService = screenTextService;
            _chooseWindowService = chooseWindowService;
            _cameraActionService = cameraActionService;
            _actionScreenService = actionScreenService;
            
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
            
            _bgService.Show(SaveService.GetCurrentBg(), null);
            
            _inGame = true;
            
            if (_currentScene == "scene_0_0" && _currentPart == 0)
            {
                CoroutineHelper.Inst.StartCoroutine(FirstInit());
                return;
            }
            
            AudioSystemService.Inst.StarPlayMusicOnLoop(MusicType.EMBIENT_SLOW);
            FadeService.FadeService.FadeOut();
            ShowScene();
        }

        IEnumerator FirstInit()
        {
            _currentSceneSo = GameModel.GetScene(_currentScene);
            _bgService.Show(_currentSceneSo.ChangeBackGround.bgEnum, null);
            
            yield return new WaitForSeconds(1.0f);
            
            AudioSystemService.Inst.StarPlayMusicOnLoop(MusicType.RADIO_COPS);
            
            yield return new WaitForSeconds(4.5f);
            
            AudioSystemService.Inst.StarPlayMusicOnLoop(MusicType.RADIO_CHANGE);
            AudioSystemService.Inst.AddQueueClipToLoop(MusicType.NEBO);
            AudioSystemService.Inst.AddQueueClipToLoop(MusicType.EMBIENT_SLOW);
            
            yield return new WaitForSeconds(2.0f);
            
            FadeService.FadeService.FadeOut(8.0f);
            
            yield return new WaitForSeconds(8.0f);
            
            ShowScene();
        }
        
        public void ShowNextScene(string key)
        {
            _currentScene = key;
            _currentPart = 0;

            SaveService.SaveScene(_currentScene);
            SaveService.SavePart(_currentPart);
            
            _characterService.HideAllCharacters();
            _screenTextService.HideText();
            _chooseWindowService.ChangeVisible(false);
            _cameraActionService.ChangeVisible(false);

            ScreenSceneScriptableObject currentSceneSo = GameModel.GetScene(_currentScene);
            
            if (currentSceneSo.ChangeBackGround.enable)
            {
                _bgService.Show(currentSceneSo.ChangeBackGround.bgEnum, ShowScene);
                // if(_changeBgRoutine != null)
                //     CoroutineHelper.Inst.StopCoroutine(_changeBgRoutine);
                //
                // _changeBgRoutine = CoroutineHelper.Inst.StartCoroutine(ChangeBgRoutine());
                
                return;
            }
            
            ShowScene();
        }

        // private IEnumerator ChangeBgRoutine()
        // {
        //     float duration = 3.0f;
        //     FadeService.FadeService.FadeIn(duration);
        //     
        //     yield return new WaitForSeconds(duration);
        //     
        //     ScreenSceneScriptableObject currentSceneSo = GameModel.GetScene(_currentScene);
        //     
        //     if(currentSceneSo.ChangeBackGround.enable)
        //     {
        //         BgEnum bgEnum = currentSceneSo.ChangeBackGround.bgEnum;
        //         _bgService.Show(bgEnum);
        //     }
        //     
        //     yield return new WaitForSeconds(1.0f);
        //     
        //     FadeService.FadeService.FadeOut(duration);
        //     
        //     yield return new WaitForSeconds(duration / 2);
        //     
        //     ShowScene();
        // }

        private void ShowScene()
        {
            _currentSceneSo = GameModel.GetScene(_currentScene);
            
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
                
                Debug.Log($"{_currentSceneSo.CountSetter.Type} = {count}");
                GameModel.SetInt(_currentSceneSo.CountSetter.Type, count);
            }
            //
            // if(_currentSceneSo.ChangeBackGround.enable)
            // {
            //     _bgService.Show(_currentSceneSo.ChangeBackGround.bgEnum);
            // }

            ShowPart();
        }
        
        public void ShowNextPart()
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
            
            _chooseWindowService.SetChooses(
                PrepareList(false, out bool isCameraAfter), 
                _currentPartSo.TextShow, 
                isCameraAfter);
        }

        private void TakePhoto()
        {
            _characterService.HideAllCharacters();
            _screenTextService.HideText();
            
            _chooseWindowService.SetChooses(
                PrepareList(true, out bool isCameraAfter), 
                _currentPartSo.TextShow, 
                isCameraAfter);
        }

        private NextScene[] PrepareList(bool isCamera, out bool isCameraAfter)
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
                choosesList.isUseCamera = isCamera;
                
                for (int i = 0; i < countChooses; i++)
                {
                    choosesList.chooseKeys[i] = _currentSceneSo.NextScenes[i].Scene.SceneKey;
                    choosesList.chooseStatus[i] = false;
                }
                
                SaveService.SetChoosesList(_currentSceneSo.SceneKey, choosesList);
            }

            if (choosesList.isUseCamera)
            {
                isCamera = true;
            }
            else
            {
                choosesList.isUseCamera = isCamera;
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

            isCameraAfter = isCamera;
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