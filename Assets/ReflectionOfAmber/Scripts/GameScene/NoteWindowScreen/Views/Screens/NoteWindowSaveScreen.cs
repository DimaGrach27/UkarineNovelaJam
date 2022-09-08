using System;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Misc;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views.Screens
{
    public class NoteWindowSaveScreen : NoteWindowScreenBase
    {
        public override NoteWindowScreensEnum NoteWindowScreensEnum => NoteWindowScreensEnum.SAVE_SCREEN;

        [SerializeField] private NoteWindowSaveScreenPart[] buttons;

        public int ButtonsCount => buttons.Length;
        
        public event Action<int> OnCLickButton;
        public event Action OnOpen;
        
        public override void Open()
        {
            base.Open();
            OnOpen?.Invoke();
        }

        private void Awake()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Index = i;
                buttons[i].OnCLickButton += OnCLickButtonHandler;
            }
        }
        
        private void OnCLickButtonHandler(int index) => OnCLickButton?.Invoke(index);

        public void UpdateElement(int index, Sprite sprite, bool isHaveSave, string description)
        {
            buttons[index].Sprite = sprite;
            buttons[index].IsHaveSave = isHaveSave;
            buttons[index].Description = description;
        }
    }
}