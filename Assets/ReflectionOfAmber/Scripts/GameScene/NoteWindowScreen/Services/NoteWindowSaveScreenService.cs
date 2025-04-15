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
            m_NoteWindowSaveScreen = noteWindowSaveScreen;
            m_ConfirmScreen = confirmScreen;

            m_NoteWindowSaveScreen.OnOpen += OnOpenHandler;
            m_NoteWindowSaveScreen.OnCLickButton += OnClickButtonHandler;
        }

        private readonly NoteWindowSaveScreen m_NoteWindowSaveScreen;
        private readonly ConfirmScreen m_ConfirmScreen;

        private int m_ConfirmIndex = -1;

        private void OnOpenHandler()
        {
            for (int i = 0; i < m_NoteWindowSaveScreen.ButtonsCount; i++)
            {
                if (SaveService.TryGetSaveGame(i, out SaveFile saveFile))
                {
                    BgEnum bgEnum = (BgEnum)saveFile.currentBg;
                    Sprite spriteBg = GameModel.GetBg(bgEnum);
                    string saveText = $"{TranslatorService.GetText(TranslatorKeys.SAVE_ON_CARD)} {i + 1}";
                    m_NoteWindowSaveScreen.UpdateElement(i, spriteBg, true, saveText);
                }
                else
                {
                    m_NoteWindowSaveScreen.UpdateElement(i, null, false, TranslatorService.GetText(TranslatorKeys.EMPTY));
                }
            }
        }

        private void OnClickButtonHandler(int index)
        {
            if (!SaveService.TryGetSaveGame(index, out SaveFile saveFile))
            {
                BgEnum bgEnum = SaveService.GetCurrentBg();
                Sprite spriteBg = GameModel.GetBg(bgEnum);
                string saveText = $"{TranslatorService.GetText(TranslatorKeys.SAVE_ON_CARD)} {index + 1}";
                m_NoteWindowSaveScreen.UpdateElement(index, spriteBg, true, saveText);
                SaveService.SaveGame(index);
            }
            else
            {
                m_ConfirmIndex = index;
                m_ConfirmScreen.Check(OnConfirmHandler, TranslatorKeys.CONFIRM_RESAVE);
            }
        }

        private void OnConfirmHandler(bool isConfirm)
        {
            if(!isConfirm) return;
            if(m_ConfirmIndex < 0) return;
            
            BgEnum bgEnum = SaveService.GetCurrentBg();
            Sprite spriteBg = GameModel.GetBg(bgEnum);
            string saveText = $"{TranslatorService.GetText(TranslatorKeys.SAVE_ON_CARD)} {m_ConfirmIndex + 1}";
            m_NoteWindowSaveScreen.UpdateElement(m_ConfirmIndex, spriteBg, true, saveText);
            SaveService.SaveGame(m_ConfirmIndex);

            m_ConfirmIndex = -1;
        }

        public void Dispose()
        {
            m_NoteWindowSaveScreen.OnOpen -= OnOpenHandler;
            m_NoteWindowSaveScreen.OnCLickButton -= OnClickButtonHandler;
        }
    }
}