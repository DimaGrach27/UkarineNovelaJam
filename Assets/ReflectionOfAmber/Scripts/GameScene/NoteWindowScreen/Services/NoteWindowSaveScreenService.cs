using System;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.BgScreen;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views.Screens;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Services
{
    public class NoteWindowSaveScreenService : IDisposable
    {
        [Inject]
        public NoteWindowSaveScreenService(
            NoteWindowSaveScreen noteWindowSaveScreen,
            ConfirmScreen confirmScreen)
        {
            _noteWindowSaveScreen = noteWindowSaveScreen;
            _confirmScreen = confirmScreen;

            _noteWindowSaveScreen.OnOpen += OnOpenHandler;
            _noteWindowSaveScreen.OnCLickButton += OnClickButtonHandler;
        }

        private readonly NoteWindowSaveScreen _noteWindowSaveScreen;
        private readonly ConfirmScreen _confirmScreen;

        private int _confirmIndex = -1;

        private void OnOpenHandler()
        {
            for (int i = 0; i < _noteWindowSaveScreen.ButtonsCount; i++)
            {
                if (SaveService.TryGetSaveGame(i, out SaveFile saveFile))
                {
                    BgEnum bgEnum = (BgEnum)saveFile.currentBg;
                    Sprite spriteBg = GameModel.GetBg(bgEnum);
                    _noteWindowSaveScreen.UpdateElement(i, spriteBg, true, $"Save {i}");
                }
                else
                {
                    _noteWindowSaveScreen.UpdateElement(i, null, false, TranslatorParser.GetText(TranslatorKeys.EMPTY));
                }
            }
        }

        private void OnClickButtonHandler(int index)
        {
            if (!SaveService.TryGetSaveGame(index, out SaveFile saveFile))
            {
                BgEnum bgEnum = SaveService.GetCurrentBg();
                Sprite spriteBg = GameModel.GetBg(bgEnum);
                _noteWindowSaveScreen.UpdateElement(index, spriteBg, true, $"Save {index}");
                SaveService.SaveGame(index);
            }
            else
            {
                _confirmIndex = index;
                _confirmScreen.Check(OnConfirmHandler, TranslatorKeys.CONFIRM_RESAVE);
            }
        }

        private void OnConfirmHandler(bool isConfirm)
        {
            if(!isConfirm) return;
            if(_confirmIndex < 0) return;
            
            BgEnum bgEnum = SaveService.GetCurrentBg();
            Sprite spriteBg = GameModel.GetBg(bgEnum);
            _noteWindowSaveScreen.UpdateElement(_confirmIndex, spriteBg, true, $"Save {_confirmIndex}");
            SaveService.SaveGame(_confirmIndex);

            _confirmIndex = -1;
        }

        public void Dispose()
        {
            
        }
    }
}