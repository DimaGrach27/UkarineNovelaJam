using System;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views.Screens;
using ReflectionOfAmber.Scripts.GameScene.Services;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.Settings;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Services
{
    public class NoteWindowSettingsScreenService : IDisposable
    {
        [Inject]
        public NoteWindowSettingsScreenService(AudioSystemService audioSystemService, 
            GlobalBrightnessService globalBrightnessService,
            NoteWindowSettingsScreenView noteWindowSettingsScreenView)
        {
            _audioSystemService = audioSystemService;
            _globalBrightnessService = globalBrightnessService;
            
            noteWindowSettingsScreenView.OnChangeSpeedText += ChangeSpeedText;
            noteWindowSettingsScreenView.OnChangeMusicVolume += ChangeMusicVolume;
            noteWindowSettingsScreenView.OnChangeSoundVolume += ChangeSoundVolume;
            noteWindowSettingsScreenView.OnChangeBrightnessValue += ChangeBrightnessValue;
        }
        
        private readonly AudioSystemService _audioSystemService;
        private readonly GlobalBrightnessService _globalBrightnessService;
        
        private void ChangeSpeedText(float value)
        {
            float valueTyping = 1.0f - value / 10;
            valueTyping /= 10;
            valueTyping = Mathf.Clamp(valueTyping, 0.01f, 0.1f);
            GameModel.TYPING_SPEED = valueTyping;
            
            SaveService.TypingSpeed = value / 10;
        }
        
        private void ChangeMusicVolume(float value)
        {
            _audioSystemService.ChangeMusic(value / 10);
            SaveService.MusicVolume = value / 10;
        }
        
                
        private void ChangeSoundVolume(float value)
        {
            _audioSystemService.ChangeAudio(value / 10);
            SaveService.AudioVolume = value / 10;
        }

        private void ChangeBrightnessValue(float value)
        {
            _globalBrightnessService.BrightnessValue = (value / 10);
            SaveService.BrightnessValue = value / 10;
        }

        public void Dispose()
        {
            
        }
    }
}