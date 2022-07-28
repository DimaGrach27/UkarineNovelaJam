using System.Collections;
using DG.Tweening;
using GameScene.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public class SettingsService : MonoBehaviour
    {
        [SerializeField] private SettingElementSlider speedText;
        [SerializeField] private SettingElementSlider musicVolume;
        [SerializeField] private SettingElementSlider soundVolume;

        [SerializeField] private Button button;

        private CanvasGroup _canvasGroup;
        private Coroutine _routine;
        
        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            
            speedText.OnChangeValue += ChangeSpeedText;
            musicVolume.OnChangeValue += ChangeMusicVolume;
            soundVolume.OnChangeValue += ChangeSoundVolume;
            
            float valueTyping = 1.0f - SaveService.GetTypingSpeed() / 10;
            valueTyping = Mathf.Clamp(valueTyping, 0.01f, 0.1f);
            GlobalConstant.TYPING_SPEED = valueTyping;
            
            speedText.SetValue(SaveService.GetTypingSpeed() * 10);
            musicVolume.SetValue(SaveService.GetMusicVolume() * 10);
            soundVolume.SetValue(SaveService.GetAudioVolume() * 10);
            
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
            valueTyping = Mathf.Clamp(valueTyping, 0.01f, 0.1f);
            GlobalConstant.TYPING_SPEED = valueTyping;
            
            SaveService.SaveTypingSpeed(value / 10);
        }
        
        private void ChangeMusicVolume(float value)
        {
            AudioSystemService.Inst.ChangeMusic(value / 10);
            SaveService.SaveMusicVolume(value / 10);
        }
        
                
        private void ChangeSoundVolume(float value)
        {
            AudioSystemService.Inst.ChangeAudio(value / 10);
            SaveService.SaveAudioVolume(value / 10);
        }

        private void Close()
        {
            if(_routine != null)
                CoroutineHelper.Inst.StopCoroutine(_routine);

            _routine = CoroutineHelper.Inst.StartCoroutine(FadeOutWindow());
        }

        private void Open()
        {
            if(_routine != null)
                CoroutineHelper.Inst.StopCoroutine(_routine);
            
            _routine = CoroutineHelper.Inst.StartCoroutine(FadeInWindow());
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