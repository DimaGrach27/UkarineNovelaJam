using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Misc;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views
{
    [RequireComponent(typeof(CanvasGroup))]
    public class NoteWindowScreenPopup : MonoBehaviour
    {
        public event Action<NoteWindowScreensEnum> OnSelectWindowClick;
        
        [SerializeField] private NoteWindowScreenButton[] buttons;
        [SerializeField] private NoteWindowScreenBgView noteWindowScreenBgView;
        
        private Dictionary<NoteWindowScreensEnum, NoteWindowScreenButton> _buttonsNoteMap;
        
        private CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                    _canvasGroup = GetComponent<CanvasGroup>();
                return _canvasGroup;
            }
        }
        private CanvasGroup _canvasGroup;
        private Tween _fade;
        
        private void Awake()
        {
            if (_buttonsNoteMap == null)
            {
                _buttonsNoteMap = new();
                foreach (var button in buttons)
                {
                    _buttonsNoteMap.Add(button.NoteWindowScreensEnum, button);
                    button.OnClickButton += OnSelectWindowHandler;
                }
            }
        }

        public void Open()
        {
            FadeInWindow();
            GlobalEvent.HideCanvas();
            if (_buttonsNoteMap == null)
            {
                _buttonsNoteMap = new();
                foreach (var button in buttons)
                {
                    _buttonsNoteMap.Add(button.NoteWindowScreensEnum, button);
                    button.OnClickButton += OnSelectWindowHandler;
                }
            }
            
            OnSelectWindowHandler(NoteWindowScreensEnum.MAIN_SCREEN);
        }
        
        public void Close()
        {
            GlobalEvent.ShowCanvas();
            FadeOutWindow();
        }
        
        private void OnSelectWindowHandler(NoteWindowScreensEnum noteWindowScreensEnum)
        {
            bool isFirstPage = noteWindowScreensEnum == NoteWindowScreensEnum.MAIN_SCREEN;
            
            noteWindowScreenBgView.FirstPage = isFirstPage;
            noteWindowScreenBgView.SetLastElement();
            _buttonsNoteMap[noteWindowScreensEnum].transform.SetAsLastSibling();
            
            OnSelectWindowClick?.Invoke(noteWindowScreensEnum);
        }
        
        private void FadeInWindow()
        {
            if (_fade != null) DOTween.Kill(_fade);
            gameObject.SetActive(true);
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
            CanvasGroup.alpha = 0.0f;
            float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION;
            _fade = CanvasGroup.DOFade(1.0f, duration);
        }
        
        private async void FadeOutWindow()
        {
            if (_fade != null) DOTween.Kill(_fade);

            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
            CanvasGroup.alpha = 1.0f;
            float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION;
            _fade = CanvasGroup.DOFade(0.0f, duration);

            await Task.Delay((int)(duration * 1000));
            gameObject.SetActive(false);
        }
    }
}