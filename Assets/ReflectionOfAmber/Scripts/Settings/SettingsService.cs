using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.Services;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ReflectionOfAmber.Scripts.Settings
{
    public class SettingsService : MonoBehaviour, IInit
    {
        public event Action OnCloseButtonClick;
        public event Action OnReady;

        [SerializeField] private TMP_Dropdown m_languageDropdown;
        
        [SerializeField] private SettingElementSlider speedText;
        [SerializeField] private SettingElementSlider musicVolume;
        [SerializeField] private SettingElementSlider soundVolume;
        [SerializeField] private SettingElementSlider brightnessValue;

        [SerializeField] private Button button;

        private AudioSystemService _audioSystemService;
        private GlobalBrightnessService _globalBrightnessService;
        private TranslatorService m_TranslatorService;
        
        private CanvasGroup _canvasGroup;
        private Coroutine _routine;
        
        [Inject]
        public void Construct(
            AudioSystemService audioSystemService,
            GlobalBrightnessService globalBrightnessService,
            TranslatorService translatorService
            )
        {
            _audioSystemService = audioSystemService;
            _globalBrightnessService = globalBrightnessService;
            m_TranslatorService = translatorService;
        }
        
        public void Init()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            float valueTyping = 1.0f - SaveService.TypingSpeed / 10;
            valueTyping = Mathf.Clamp(valueTyping, 0.01f, 0.1f);
            GameModel.TYPING_SPEED = valueTyping;

            speedText.SetValue(SaveService.TypingSpeed * 10);
            musicVolume.SetValue(SaveService.MusicVolume * 10);
            soundVolume.SetValue(SaveService.AudioVolume * 10);
            brightnessValue.SetValue(SaveService.BrightnessValue * 10);

            _audioSystemService.ChangeMusic(SaveService.MusicVolume);
            _audioSystemService.ChangeAudio(SaveService.AudioVolume);
            _globalBrightnessService.BrightnessValue = SaveService.BrightnessValue;
            
            button.onClick.AddListener(Close);

            speedText.OnChangeValue += ChangeSpeedText;
            musicVolume.OnChangeValue += ChangeMusicVolume;
            soundVolume.OnChangeValue += ChangeSoundVolume;
            brightnessValue.OnChangeValue += ChangeBrightnessValue;

            _canvasGroup.interactable = false;
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;

            InitLanguage();
            
            OnReady?.Invoke();
        }

        private void InitLanguage()
        {
            List<TMP_Dropdown.OptionData> options = new();
            foreach (TranslatorLanguages langName in Enum.GetValues(typeof(TranslatorLanguages)))
            {
                string langNameTemp = string.Empty;

                switch (langName)
                {
                    case TranslatorLanguages.UKR:
                        langNameTemp = "Українська";
                        break;
                    case TranslatorLanguages.ENG:
                        langNameTemp = "English";
                        break;
                }

                options.Add(new()
                {
                    text = langNameTemp
                });
            }

            m_languageDropdown.options = options;
            m_languageDropdown.SetValueWithoutNotify((int)GameModel.CurrentLanguage);
            m_languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
        }

        private void OnLanguageChanged(int langIndex)
        {
            m_TranslatorService.ChangeLanguage((TranslatorLanguages)langIndex);
        }

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
        
        private void Close()
        {
            if(_routine != null)
                StopCoroutine(_routine);

            _routine = StartCoroutine(FadeOutWindow());
            OnCloseButtonClick?.Invoke();
        }

        public void Open()
        {
            if(_routine != null)
                StopCoroutine(_routine);
            
            _routine = StartCoroutine(FadeInWindow());
        }
        
        private IEnumerator FadeInWindow()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION;
            _canvasGroup.DOFade(1.0f, duration);
            yield return null;
        }
        
        private IEnumerator FadeOutWindow()
        {
            _canvasGroup.interactable = false;
            float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION;
            _canvasGroup.DOFade(0.0f, duration);
            yield return new WaitForSeconds(duration);
            _canvasGroup.blocksRaycasts = false;
        }
    }
}