using System;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Misc;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.Settings;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views.Screens
{
    public class NoteWindowSettingsScreenView : NoteWindowScreenBase
    {
        public override NoteWindowScreensEnum NoteWindowScreensEnum => NoteWindowScreensEnum.SETTINGS_SCREEN;

        [SerializeField] private SettingElementSlider speedText;
        [SerializeField] private SettingElementSlider musicVolume;
        [SerializeField] private SettingElementSlider soundVolume;
        [SerializeField] private SettingElementSlider brightnessValue;

        public event Action<float> OnChangeSpeedText; 
        public event Action<float> OnChangeMusicVolume; 
        public event Action<float> OnChangeSoundVolume; 
        public event Action<float> OnChangeBrightnessValue;

        public override void Open()
        {
            base.Open();
            
            speedText.SetValue(SaveService.TypingSpeed * 10);
            musicVolume.SetValue(SaveService.MusicVolume * 10);
            soundVolume.SetValue(SaveService.AudioVolume * 10);
            brightnessValue.SetValue(SaveService.BrightnessValue * 10);
        }

        private void Start()
        {
            speedText.OnChangeValue += ChangeSpeedText;
            musicVolume.OnChangeValue += ChangeMusicVolume;
            soundVolume.OnChangeValue += ChangeSoundVolume;
            brightnessValue.OnChangeValue += ChangeBrightnessValue;
            
            float valueTyping = 1.0f - SaveService.TypingSpeed / 10;
            valueTyping = Mathf.Clamp(valueTyping, 0.01f, 0.1f);
            GameModel.TYPING_SPEED = valueTyping;
        }

        private void ChangeSpeedText(float value)
        {
            OnChangeSpeedText?.Invoke(value);
        }
        
        private void ChangeMusicVolume(float value)
        {
            OnChangeMusicVolume?.Invoke(value);
        }

        private void ChangeSoundVolume(float value)
        {
            OnChangeSoundVolume?.Invoke(value);
        }

        private void ChangeBrightnessValue(float value)
        {
            OnChangeBrightnessValue?.Invoke(value);
        }
    }
}