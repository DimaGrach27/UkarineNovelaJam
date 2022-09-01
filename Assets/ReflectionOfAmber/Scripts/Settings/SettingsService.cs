using System.Collections;
using DG.Tweening;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene;
using ReflectionOfAmber.Scripts.GameScene.Services;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ReflectionOfAmber.Scripts.Settings
{
    public class SettingsService : MonoBehaviour
    {
        [SerializeField] private SettingElementSlider speedText;
        [SerializeField] private SettingElementSlider musicVolume;
        [SerializeField] private SettingElementSlider soundVolume;
        [SerializeField] private SettingElementSlider brightnessValue;

        [SerializeField] private Button button;

        private AudioSystemService _audioSystemService;
        private GlobalBrightnessService _globalBrightnessService;
        
        private CanvasGroup _canvasGroup;
        private Coroutine _routine;

        [Inject]
        public void Construct(AudioSystemService audioSystemService, GlobalBrightnessService globalBrightnessService)
        {
            _audioSystemService = audioSystemService;
            _globalBrightnessService = globalBrightnessService;
        }
        
        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            
            speedText.OnChangeValue += ChangeSpeedText;
            musicVolume.OnChangeValue += ChangeMusicVolume;
            soundVolume.OnChangeValue += ChangeSoundVolume;
            brightnessValue.OnChangeValue += ChangeBrightnessValue;
            
            float valueTyping = 1.0f - SaveService.TypingSpeed / 10;
            valueTyping = Mathf.Clamp(valueTyping, 0.01f, 0.1f);
            GameModel.TYPING_SPEED = valueTyping;
            
            speedText.SetValue(SaveService.TypingSpeed * 10);
            musicVolume.SetValue(SaveService.MusicVolume * 10);
            soundVolume.SetValue(SaveService.AudioVolume * 10);
            brightnessValue.SetValue(SaveService.BrightnessValue * 10);
            
            button.onClick.AddListener(Close);
            
            GlobalEvent.OnCallType += Open;
            
            _canvasGroup.interactable = false;
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
        }

        private void Open(CallKeyType type)
        {
            if(type != CallKeyType.OPEN_SETTINGS) return;
            Open();
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
        }

        private void Open()
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