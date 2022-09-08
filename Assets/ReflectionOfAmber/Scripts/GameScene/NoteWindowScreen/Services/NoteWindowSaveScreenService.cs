using ReflectionOfAmber.Scripts.GameScene.BgScreen;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views.Screens;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Services
{
    public class NoteWindowSaveScreenService
    {
        [Inject]
        public NoteWindowSaveScreenService(NoteWindowSaveScreen noteWindowSaveScreen,
            BgService bgService,
            ConfirmScreen confirmScreen)
        {
            _noteWindowSaveScreen = noteWindowSaveScreen;
            _confirmScreen = confirmScreen;
            _bgService = bgService;

            _noteWindowSaveScreen.OnOpen += OnOpenHandler;
            _noteWindowSaveScreen.OnCLickButton += OnClickButtonHandler;
        }

        private readonly NoteWindowSaveScreen _noteWindowSaveScreen;
        private readonly ConfirmScreen _confirmScreen;
        private readonly BgService _bgService;

        private int _confirmIndex = -1;

        private void OnOpenHandler()
        {
            for (int i = 0; i < _noteWindowSaveScreen.ButtonsCount; i++)
            {
                if (SaveService.TryGetSaveGame(i, out SaveFile saveFile))
                {
                    BgEnum bgEnum = (BgEnum)saveFile.currentBg;
                    Sprite spriteBg = _bgService.GetBg(bgEnum);
                    _noteWindowSaveScreen.UpdateElement(i, spriteBg, true, $"Save {i}");
                }
                else
                {
                    _noteWindowSaveScreen.UpdateElement(i, null, false, $"Пусто");
                }
            }
        }

        private void OnClickButtonHandler(int index)
        {
            if (!SaveService.TryGetSaveGame(index, out SaveFile saveFile))
            {
                BgEnum bgEnum = SaveService.GetCurrentBg();
                Sprite spriteBg = _bgService.GetBg(bgEnum);
                _noteWindowSaveScreen.UpdateElement(index, spriteBg, true, $"Save {index}");
                SaveService.SaveGame(index);
            }
            else
            {
                _confirmIndex = index;
                _confirmScreen.Check(OnConfirmHandler, "Ви точно бажаєте переписати збереження?");
            }
        }

        private void OnConfirmHandler(bool isConfirm)
        {
            if(!isConfirm) return;
            if(_confirmIndex < 0) return;
            
            BgEnum bgEnum = SaveService.GetCurrentBg();
            Sprite spriteBg = _bgService.GetBg(bgEnum);
            _noteWindowSaveScreen.UpdateElement(_confirmIndex, spriteBg, true, $"Save {_confirmIndex}");
            SaveService.SaveGame(_confirmIndex);

            _confirmIndex = -1;
        }
    }
}