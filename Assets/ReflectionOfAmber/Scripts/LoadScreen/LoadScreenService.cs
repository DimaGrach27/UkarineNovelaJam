using System;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.BgScreen;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.LoadScreen
{
    public class LoadScreenService
    {
        private readonly LoadScreenView _loadScreenView;
        private readonly ConfirmScreen _confirmScreen;
        private readonly SceneService _sceneService;

        public event Action OnCloseClick;
        
        [Inject]
        public LoadScreenService(LoadScreenView loadScreenView,
            ConfirmScreen confirmScreen,
            SceneService sceneService)
        {
            _loadScreenView = loadScreenView;
            _confirmScreen = confirmScreen;
            _sceneService = sceneService;
            
            loadScreenView.OnCLickButton += OnClickToCardHandler;
            loadScreenView.OnCloseClick += OnCloseClickHandler;

            _hasSaveArray = new bool[_loadScreenView.Count];
        }

        private readonly bool[] _hasSaveArray;
        private int _confirmIndex = -1;

        public void Open()
        {
            _loadScreenView.Open();
            
            for (int i = 0; i < _loadScreenView.Count; i++)
            {
                if (SaveService.TryGetSaveGame(i, out SaveFile saveFile))
                {
                    _hasSaveArray[i] = true;
                    BgEnum bgEnum = (BgEnum)saveFile.currentBg;
                    Sprite spriteBg = GameModel.GetBg(bgEnum);
                    _loadScreenView.UpdateElement(i, spriteBg, true, $"Save {i}");
                }
                else
                {
                    _hasSaveArray[i] = false;
                    _loadScreenView.UpdateElement(i, null, false, TranslatorParser.GetText(TranslatorKeys.EMPTY));
                }
            }
        }

        private void OnClickToCardHandler(int index)
        {
            if(!_hasSaveArray[index]) return;
            _confirmIndex = index;
            _confirmScreen.Check(ConfirmLoad, TranslatorKeys.CONFIRM_NEW_GAME);
        }

        private void ConfirmLoad(bool isConfirm)
        {
            if (isConfirm)
            {
                SaveService.SetLoadGame(_confirmIndex);
                _sceneService.LoadGameScene();
            }

            _confirmIndex = -1;
        }

        private void OnCloseClickHandler() => OnCloseClick?.Invoke();
    }
}