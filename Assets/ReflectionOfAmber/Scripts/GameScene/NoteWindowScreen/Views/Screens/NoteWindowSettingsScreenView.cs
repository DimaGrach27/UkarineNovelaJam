using System;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using TMPro;
using Zenject;
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

        [SerializeField] private TMP_Dropdown m_dialogInputDropdown;
        [SerializeField] private TMP_Text m_dialogInputLabel;

        private TranslatorService m_translatorService;
        private uint? m_languageSubscription;

        [Inject]
        public void Construct(TranslatorService translatorService)
        {
            m_translatorService = translatorService;
            m_languageSubscription = translatorService.Subscribe(this, RefreshDialogInput);
            translatorService.OnReady += RefreshDialogInput;
        }

        public event Action<bool> OnChangeDialogInput;
        public event Action<float> OnChangeSpeedText; 
        public event Action<float> OnChangeMusicVolume; 
        public event Action<float> OnChangeSoundVolume; 
        public event Action<float> OnChangeBrightnessValue;

        public override void Open()
        {
            base.Open();
            RefreshDialogInput();
            
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
            m_dialogInputDropdown.onValueChanged.AddListener(ChangeDialogInput);
            RefreshDialogInput();
            
            float valueTyping = 1.0f - SaveService.TypingSpeed / 10;
            valueTyping = Mathf.Clamp(valueTyping, 0.01f, 0.1f);
            GameModel.TYPING_SPEED = valueTyping;
        }

        private void RefreshDialogInput()
        {
            m_dialogInputLabel.text = TranslatorService.GetText(TranslatorKeys.ADVANCE_DIALOG);
            m_dialogInputDropdown.options = new List<TMP_Dropdown.OptionData>
            {
                new(TranslatorService.GetText(TranslatorKeys.ADVANCE_DIALOG_UI)),
                new(TranslatorService.GetText(TranslatorKeys.ADVANCE_DIALOG_UI_KEYS))
            };
            m_dialogInputDropdown.SetValueWithoutNotify(SaveService.DialogKeyboardMouseEnabled ? 1 : 0);
            m_dialogInputDropdown.RefreshShownValue();
        }

        private void ChangeDialogInput(int index)
        {
            OnChangeDialogInput?.Invoke(index == 1);
        }

        private void OnDestroy()
        {
            if (m_languageSubscription.HasValue)
            {
                m_translatorService.Unsubscribe(m_languageSubscription.Value);
                m_translatorService.OnReady -= RefreshDialogInput;
            }

            m_dialogInputDropdown.onValueChanged.RemoveListener(ChangeDialogInput);
            speedText.OnChangeValue -= ChangeSpeedText;
            musicVolume.OnChangeValue -= ChangeMusicVolume;
            soundVolume.OnChangeValue -= ChangeSoundVolume;
            brightnessValue.OnChangeValue -= ChangeBrightnessValue;
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